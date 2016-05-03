//
// Copyright (c) 2013 Augmentum, Inc. All Rights Reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//

//using Augmentum.XGenos.Data.Migration.Schema;
//using Augmentum.XGenos.Utility;

namespace Finance.Framework.Data
{
    public interface IPropertyAware
    {
        string CompanyCode { get; set; }
        string PropertyCode { get; set; }
    }

    public abstract class PropertyAwareRecord : IPropertyAware
    {
        /// <summary>
        /// Gets or sets Company code to mark data from diffrent companies in DB
        /// </summary>
        public virtual string CompanyCode { get; set; }

        /// <summary>
        /// Gets or sets Property code to mark data from diffrent properties in DB
        /// </summary>
        public virtual string PropertyCode { get; set; }
    }

    //public static class PropertyAwareRecordExtension
    //{
    //    /// <summary>
    //    /// Defines two columns to support multiple properties.
    //    /// </summary>
    //    public static CreateTableCommand PropertyAwareRecord(this CreateTableCommand command)
    //    {
    //        command
    //            .Column<string>(ReflectOn<PropertyAwareRecord>.NameOf(x => x.CompanyCode), c => c.WithLength(100).NotNull())
    //            .Column<string>(ReflectOn<PropertyAwareRecord>.NameOf(x => x.PropertyCode), c => c.WithLength(100).NotNull());

    //        return command;
    //    }

    //    public static AlterTableCommand PropertyAwareRecord(this AlterTableCommand command)
    //    {
    //        command.AddColumn<string>(ReflectOn<PropertyAwareRecord>.NameOf(x => x.CompanyCode), c => c.WithLength(100).NotNull());
    //        command.AddColumn<string>(ReflectOn<PropertyAwareRecord>.NameOf(x => x.PropertyCode), c => c.WithLength(100).NotNull());

    //        return command;
    //    }
    //}
}
