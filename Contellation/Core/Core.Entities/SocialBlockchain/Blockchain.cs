using Core.Functions.Services;

namespace Core.Entities.SocialBlockchain
{
    public class Blockchain
    {
        private List<Block> chain;
        private int difficulty = 2;
        public int TokenBalance { get; private set; } = 100;

        public Blockchain()
        {
            //chain = new List<Block> { new Block() { 0, "Genesis Block", "0", "system", "public" } };
        }

        public void AddBlock(string data, string ownerId, string visibility)
        {
            var lastBlock = chain.Last();
            //var newBlock = new Block(chain.Count, data, lastBlock.Hash, ownerId, visibility){};
            var newBlock = new Block() { };
            newBlock.Mine(difficulty);
            chain.Add(newBlock);
        }

        public List<Block> GetChain(string viewerId, string viewerRole)
        {
            return chain.Where(b => Auth.checkPermission(viewerRole, b.Visibility)).ToList();
        }

        public bool SpendTokens(int amount)
        {
            if (TokenBalance >= amount)
            {
                TokenBalance -= amount;
                AddBlock($"Spent {amount} tokens", "system", "public");
                return true;
            }
            return false;
        }
    }
}
