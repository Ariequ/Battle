using System.Collections.Generic;

public class LimitFunctionTreeFactory
{
    private static LimitFunctionTreeFactory _instance;

    public static LimitFunctionTreeFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LimitFunctionTreeFactory();
            }

            return _instance;
        }
    }

    public LimitFunctionNode Create(int nodeID)
    {
        ILimit limit = LimitFactory.Instance.Create(nodeID);
        
        List<LimitFunctionNode> list1 = new List<LimitFunctionNode>();
        list1.Add(new LimitFunctionNode(1, null, null));

        List<LimitFunctionNode> list2 = new List<LimitFunctionNode>();
        list2.Add(new LimitFunctionNode(2, null, list1));
        list2.Add(new LimitFunctionNode(2, null, list1));
        list2.Add(new LimitFunctionNode(2, null, list1));
        list2.Add(new LimitFunctionNode(2, null, list1));

        LimitFunctionNode node = new LimitFunctionNode(1, limit, list2);

        return node;
    }
}
