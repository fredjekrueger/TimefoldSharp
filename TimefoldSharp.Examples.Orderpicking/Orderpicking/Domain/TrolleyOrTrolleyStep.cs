using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Domain.Entity;
using TimefoldSharp.Core.API.Domain.Variable;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain
{
    [PlanningEntity]
    public abstract class TrolleyOrTrolleyStep
    {
        public const string PREVIOUS_ELEMENT = "PreviousElement";

        [InverseRelationShadowVariable(SourceVariableName = PREVIOUS_ELEMENT)]
        public TrolleyStep NextElement { get; set; }

        protected TrolleyOrTrolleyStep()
        {
            //marshalling constructor
        }

        public abstract WarehouseLocation GetLocation();

    }
}
