using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Layer;

namespace Business_Layer.Abstract
{
    public interface IMessageService
    {
        List<Message> GetAll();
        Message GetById(int id);
        Message GetFirstOrDefault(int id);
        void Add(Message message);
        void Update(Message message);
        void Delete(int id);
        (List<Message> messages, int totalCount) GetMessagesWithPaging(int pageNumber, int pageSize);
    }
}
