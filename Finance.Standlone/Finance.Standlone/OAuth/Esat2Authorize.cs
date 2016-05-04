using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Framework.OAuth
{
    public class Esat2Authorize : IAuthorize
    {
        public Response Authorize(string userName)
        {
            //var service = EsatClientFactory.Instance.GetEsService();
            //try
            //{
            //    service.isActive(userName);
            //    ICollection<Icgnet.Es.data.Role> roles = service.getAllRolesForAlias(userName);
            //    IEnumerable<string> list = ExtractRoleList(roles);
            //    return GetResponseForRoles(list);
            //}
            //catch (Exception ex)
            //{
            //    return new Response
            //    {
            //        IsValid = false,
            //        Message = ex.Message,
            //    };
            //}
            Response result = new Response();
            result.IsValid = true;
            result.Output = "Hello, Esat OAuth";
            return result;
        }
        
        //private IEnumerable<string> ExtractRoleList(ICollection<Icgnet.Es.data.Role> roles)
        //{
        //    IList<string> list = new List<string>();
        //    if (null != roles)
        //    {
        //        foreach (var role in roles)
        //        {
        //            list.Add(role.Id);
        //        }
        //    }

        //    return list;
        //}

        private Response GetResponseForRoles(IEnumerable<string> list)
        {
            string[] cd = list.ToArray();
            return new Response
            {
                IsValid = true,
                Message = null,
                Output = cd,
            };
        }
    }
}
