﻿using NetControl4BioMed.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetControl4BioMed.Data.Models
{
    /// <summary>
    /// Represents the database model of an analysis.
    /// </summary>
    public class Analysis
    {
        /// <summary>
        /// Gets or sets the unique internal ID of the analysis.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the date when the analysis has started.
        /// </summary>
        public DateTime? DateTimeStarted { get; set; }

        /// <summary>
        /// Gets or sets the date when the analysis has ended.
        /// </summary>
        public DateTime? DateTimeEnded { get; set; }

        /// <summary>
        /// Gets or sets the intervals in which the analysis has been running, with an underlying format of List&lt;DateTimeInterval&gt;.
        /// </summary>
        public string DateTimeIntervals { get; set; }

        /// <summary>
        /// Gets or sets the name of the analysis.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the analysis.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the current status of the analysis.
        /// </summary>
        public AnalysisStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the message log of the analysis, with an underlying format of List&lt;string&gt;.
        /// </summary>
        public string Log { get; set; }

        /// <summary>
        /// Gets or sets the current progress of the analysis.
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// Gets or sets the algorithm used by the analysis.
        /// </summary>
        public AnalysisAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the parameters of the algorithm used by the analysis, with an underlying format of AlgorithmParameters.
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// Gets or sets the users which have access to the analysis.
        /// </summary>
        public ICollection<AnalysisUser> AnalysisUsers { get; set; }

        /// <summary>
        /// Gets or sets the nodes which appear in the network corresponding to the analysis.
        /// </summary>
        public ICollection<AnalysisNode> AnalysisNodes { get; set; }

        /// <summary>
        /// Gets or sets the edges which appear in the network corresponding to the analysis.
        /// </summary>
        public ICollection<AnalysisEdge> AnalysisEdges { get; set; }

        /// <summary>
        /// Gets or sets the networks which form the network corresponding to the analysis.
        /// </summary>
        public ICollection<AnalysisNetwork> AnalysisNetworks { get; set; }

        /// <summary>
        /// Gets or sets the node collections which are used by the analysis.
        /// </summary>
        public ICollection<AnalysisNodeCollection> AnalysisNodeCollections { get; set; }

        /// <summary>
        /// Gets or sets the control paths found by the analysis.
        /// </summary>
        public ICollection<ControlPath> ControlPaths { get; set; }
    }
}
