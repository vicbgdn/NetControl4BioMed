﻿using System.Collections.Generic;

namespace NetControl4BioMed.Helpers.InputModels
{
    /// <summary>
    /// Represents the input model for a node collection.
    /// </summary>
    public class NodeCollectionInputModel
    {
        /// <summary>
        /// Represents the ID of the node collection.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Represents the name of the node collection.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the description of the node collection.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents the node collection types of the node collection.
        /// </summary>
        public IEnumerable<NodeCollectionTypeInputModel> NodeCollectionTypes { get; set; }

        /// <summary>
        /// Represents the node collection databases of the node collection.
        /// </summary>
        public IEnumerable<NodeCollectionDatabaseInputModel> NodeCollectionDatabases { get; set; }

        /// <summary>
        /// Represents the node collection nodes of the node collection.
        /// </summary>
        public IEnumerable<NodeCollectionNodeInputModel> NodeCollectionNodes { get; set; }

        /// <summary>
        /// Represents the network node collections of the node collection.
        /// </summary>
        public IEnumerable<NetworkNodeCollectionInputModel> NetworkNodeCollections { get; set; }

        /// <summary>
        /// Represents the analysis node collections of the node collection.
        /// </summary>
        public IEnumerable<AnalysisNodeCollectionInputModel> AnalysisNodeCollections { get; set; }
    }
}
