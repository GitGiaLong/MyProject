namespace Core.Entities.SocialBlockchain
{
    public class Block
    {
        private int index;
        public int Index { get { return index; } }

        private string data = string.Empty;
        public string Data { get; }

        private string previousHash = string.Empty;
        public string PreviousHash { get; }

        private string hash = string.Empty;
        public string Hash { get { return hash = CalculateHash(); } private set{ hash = value; } }

        private DateTime timestamp = DateTime.Now;
        public DateTime Timestamp { get{ return timestamp; } }

        private int nonce = new int();
        public int Nonce { get{ return nonce; } set{ nonce = value; } }

        private string ownerId = string.Empty;
        public string OwnerId { get{ return ownerId; } }

        private string visibility = string.Empty;
        public string Visibility { get{ return visibility; } } // public, friends, family

        //public Block(int index, string data, string previousHash, string ownerId, string visibility)
        //{
        //    Index = index;
        //    Data = data;
        //    PreviousHash = previousHash;
        //    OwnerId = ownerId;
        //    Visibility = visibility;
        //    Nonce = 0;
        //    Hash = CalculateHash();
        //}

        public string CalculateHash()
        {
            string input = $"{Index}{Data}{PreviousHash}{Timestamp.Ticks}{Nonce}{OwnerId}{Visibility}";
            return input.GetHashCode().ToString("X");
        }

        public void Mine(int difficulty)
        {
            string target = new string('0', difficulty);
            while (Hash.Substring(0, Math.Min(difficulty, Hash.Length)) != target)
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }
    }
}
