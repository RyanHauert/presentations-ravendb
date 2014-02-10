// Thanks to this gist: https://gist.github.com/jtbennett/2012016

using System;
using System.Linq;
using NLog;
using Raven.Abstractions.Data;
using Raven.Bundles.Replication.Plugins;
using Raven.Json.Linq;

namespace RavenDBPlugins
{
    public class LastInWinsReplicationConflictResolver
        : AbstractDocumentReplicationConflictResolver
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        public override bool TryResolve(
            string id,
            RavenJObject metadata,
            RavenJObject document,
            JsonDocument existingDoc,
            Func<string, JsonDocument> getDocument)
        {
            if (ExistingDocShouldWin(metadata, existingDoc))
            {
                ReplaceValues(metadata, existingDoc.Metadata);
                ReplaceValues(document, existingDoc.DataAsJson);
                log.Info("Replication conflict for '{0}' resolved by choosing existing document.", id);
            }
            else
            {
                log.Info("Replication conflict for '{0}' resolved by choosing inbound document.", id);
            }

            return true;
        }

        private bool ExistingDocShouldWin(RavenJObject newMetadata, JsonDocument existingDoc)
        {
            if (existingDoc == null ||
                ExistingDocHasConflict(existingDoc) ||
                ExistingDocIsOlder(newMetadata, existingDoc))
            {
                return false;
            }

            return true;
        }

        private bool ExistingDocHasConflict(JsonDocument existingDoc)
        {
            return existingDoc.Metadata[Constants.RavenReplicationConflict] != null;
        }

        private bool ExistingDocIsOlder(RavenJObject newMetadata, JsonDocument existingDoc)
        {
            var newLastModified = GetLastModified(newMetadata);

            if (!existingDoc.LastModified.HasValue ||
                newLastModified.HasValue &&
                existingDoc.LastModified <= newLastModified)
            {
                return true;
            }

            return false;
        }

        private DateTime? GetLastModified(RavenJObject metadata)
        {
            var lastModified = metadata[Constants.LastModified];

            return (lastModified == null)
                ? new DateTime?()
                : lastModified.Value<DateTime?>();
        }

        private void ReplaceValues(RavenJObject target, RavenJObject source)
        {
            var targetKeys = target.Keys.ToArray();
            foreach (var key in targetKeys)
            {
                target.Remove(key);
            }

            foreach (var key in source.Keys)
            {
                target.Add(key, source[key]);
            }
        }
    }
}