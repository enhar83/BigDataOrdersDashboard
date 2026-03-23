using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.Abstract;
using Data_Layer.Abstract;
using Entity_Layer;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace Business_Layer.Concrete
{
    public class MessageManager : IMessageService
    {
        private readonly IUnitOfWork _uow;

        public MessageManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Add(Message message)
        {
            _uow.Messages.Add(message);
            _uow.Save();

        }

        public void Delete(int id)
        {
            _uow.Messages.Delete(id);
            _uow.Save();
        }

        public List<Message> GetAll()
        {
            return _uow.Messages.GetAll()
                .ToList();
        }

        public Message GetById(int id)
        {
            return _uow.Messages.GetById(id);
        }

        public Message GetFirstOrDefault(int id)
        {
            return _uow.Messages.GetFirstOrDefault(m=>m.MessageId==id,m=>m.Customer);
        }

        public (List<Message> messages, int totalCount) GetMessagesWithPaging(int pageNumber, int pageSize)
        {
            return _uow.Messages.GetAllWithPaging(pageNumber, pageSize, m=>m.Customer);
        }

        public void Update(Message message)
        {
            _uow.Messages.Update(message);
            _uow.Save();
        }
    }
}
