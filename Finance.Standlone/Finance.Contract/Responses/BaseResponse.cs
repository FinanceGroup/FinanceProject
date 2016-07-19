using System.Collections.Generic;

using Finance.Contract.Bos;

namespace Finance.Contract.Responses
{
    public class BaseResponse<T> where T : BaseBo
    {
        public string Message { get; set; }
        public IEnumerable<T> Bos { get; set; }
        public bool IsSuccess { get; set; }
    }
}
