//
// Copyright (c) 2015 Augmentum, Inc. All Rights Reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Finance.Standlone.Conventions
{
    public class IdConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Length(36);
            instance.GeneratedBy.UuidHex("d");
        }
    }
}