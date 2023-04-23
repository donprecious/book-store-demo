namespace BookStore.Domain.Enums;

public static class EnumConverter
{
  

        public static TransactionStatus ToTransactionStatus(string v)
        {
            if(string.IsNullOrEmpty(v)) return TransactionStatus.NONE;
            
            var isSuccess = Enum.TryParse(v, true, out TransactionStatus result);
            if (!isSuccess) return TransactionStatus.NONE;
            return result;
        }
        
        public static TransactionCategory ToTransactionCategory(string v)
        {
            if(string.IsNullOrEmpty(v)) return TransactionCategory.NONE;
            
            var isSuccess = Enum.TryParse(v, true, out TransactionCategory result);
            if (!isSuccess) return TransactionCategory.NONE;
            return result;
        }
        
}