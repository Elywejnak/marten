﻿using System;
using System.Collections.Generic;
using Marten.Events;
using Marten.Generation;
using Marten.Linq;
using Marten.Linq.QueryHandlers;
using Marten.Schema.Sequences;
using Remotion.Linq;

namespace Marten.Schema
{
    public interface IDocumentSchema
    {
        /// <summary>
        /// The original StoreOptions used to configure this DocumentStore
        /// </summary>
        StoreOptions StoreOptions { get; }

        /// <summary>
        /// Retrieves or generates the active IDocumentStorage object
        /// for the given document type
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        IDocumentStorage StorageFor(Type documentType);

        /// <summary>
        /// Fetches a list of all of the Marten generated tables
        /// in the database
        /// </summary>
        /// <returns></returns>
        TableName[] SchemaTables();

        /// <summary>
        /// Fetches a list of the Marten document tables
        /// in the database
        /// </summary>
        /// <returns></returns>
        TableName[] DocumentTables();

        /// <summary>
        /// Fetches a list of functions generated by Marten
        /// in the database
        /// </summary>
        /// <returns></returns>
        FunctionName[] SchemaFunctionNames();

        /// <summary>
        /// Finds or creates the IDocumentMapping for a document type
        /// that governs how that document type is persisted and queried
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        IDocumentMapping MappingFor(Type documentType);

        /// <summary>
        /// Ensures that the IDocumentStorage object for a document type is ready
        /// and also attempts to update the database schema for any detected changes
        /// </summary>
        /// <param name="documentType"></param>
        void EnsureStorageExists(Type documentType);

        /// <summary>
        /// Used to create new Hilo sequences 
        /// </summary>
        ISequences Sequences { get; }

        /// <summary>
        /// The event store configuration
        /// </summary>
        IEventStoreConfiguration Events { get; }

        /// <summary>
        /// Access to Linq parsing for adhoc querying techniques
        /// </summary>
        MartenExpressionParser Parser { get; }

        /// <summary>
        /// Write the SQL script to build the database schema
        /// objects to a file
        /// </summary>
        /// <param name="filename"></param>
        void WriteDDL(string filename);


        /// <summary>
        /// Write all the SQL scripts to build the database schema, but
        /// split by document type
        /// </summary>
        /// <param name="directory"></param>
        void WriteDDLByType(string directory);

        /// <summary>
        /// Creates all the SQL script that would build all the database
        /// schema objects for the configured schema
        /// </summary>
        /// <returns></returns>
        string ToDDL();

        /// <summary>
        /// Fetch the actual table model for a document type
        /// </summary>
        /// <param name="documentMapping"></param>
        /// <returns></returns>
        TableDefinition TableSchema(IDocumentMapping documentMapping);

        /// <summary>
        /// Fetch the actual table model for a document type
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        TableDefinition TableSchema(Type documentType);


        IEnumerable<IDocumentMapping> AllDocumentMaps();


        IResolver<T> ResolverFor<T>();

        bool TableExists(TableName table);

        /// <summary>
        /// Used to create IQueryHandler's for Linq queries
        /// </summary>
        IQueryHandlerFactory HandlerFactory { get; }
    }
}