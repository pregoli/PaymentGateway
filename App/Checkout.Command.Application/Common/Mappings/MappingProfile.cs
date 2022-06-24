using AutoMapper;
using Checkout.Command.Application.Common.Dto;
using Checkout.Domain.Transaction;

namespace Checkout.Command.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapTransaction();
        }

        private void MapTransaction()
        {
            CreateMap<Transaction, TransactionHistoryDto>();
            CreateMap<TransactionHistoryDto, Transaction>();
        }
    }
}
