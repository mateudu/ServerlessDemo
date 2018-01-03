using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ServerlessDemo.Database.Migrator.Infrastructure.DataAccess
{
    public class ForeignKeyNamingConvention : IStoreModelConvention<AssociationType>
    {
        public void Apply(AssociationType association, DbModel model)
        {
            // Identify ForeignKey properties (including IAs)  
            if (!association.IsForeignKey) return;

            // rename FK columns  
            var constraint = association.Constraint;
            if (DoPropertiesHaveDefaultNames(constraint.FromProperties, constraint.ToProperties))
            {
                NormalizeForeignKeyProperties(constraint.FromProperties);
            }

            if (DoPropertiesHaveDefaultNames(constraint.ToProperties, constraint.FromProperties))
            {
                NormalizeForeignKeyProperties(constraint.ToProperties);
            }
        }

        private static bool DoPropertiesHaveDefaultNames(IReadOnlyList<EdmProperty> properties, IReadOnlyList<EdmProperty> otherEndProperties)
        {
            if (properties.Count != otherEndProperties.Count)
            {
                return false;
            }

            for (var i = 0; i < properties.Count; ++i)
            {
                if (properties[i].Name.Replace("_", "") != otherEndProperties[i].Name)
                {
                    return false;
                }
            }

            return true;
        }

        private void NormalizeForeignKeyProperties(ReadOnlyMetadataCollection<EdmProperty> properties)
        {
            for (var i = 0; i < properties.Count; ++i)
            {
                var underscoreIndex = properties[i].Name.IndexOf('_');
                if (underscoreIndex > 0)
                {
                    properties[i].Name = properties[i].Name.Remove(underscoreIndex, 1);
                }
            }
        }
    }
}