using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.ConversationSpecs.MessagesInConversation
{
    public class ConversationWithMessagesSpecsParams
    {
        public long ConversationId;

        private int pageNumber = 1;

        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value <= 0 ? 1 : value; }
        }

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 10 || value <= 0 ? 10 : value; }
        }

    }
}
