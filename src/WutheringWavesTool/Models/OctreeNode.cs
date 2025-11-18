using Haiyu.Helpers;

namespace Haiyu.Models
{
    public class OctreeNode
    {
        public OctreeNode[] Children = new OctreeNode[8]; // 8个子节点
        public int RSum,
            GSum,
            BSum;
        public int Count;
        public int Depth;

        public OctreeNode Parent { get; set; }
        public int ChildIndex { get; set; }

        public Color GetAverageColor()
        {
            return Color.FromArgb(
                255,
                (byte)(RSum / Count),
                (byte)(GSum / Count),
                (byte)(BSum / Count)
            );
        }

        public void Insert(Color color)
        {
            if (Depth >= OctreeColorExtractor.MAX_DEPTH)
                return;

            // 计算子节点索引
            int index = 0;
            for (int i = 0; i < 3; i++)
            {
                int shift = 7 - Depth;
                int mask = 1 << shift;
                byte component = (i == 0) ? color.R : (i == 1 ? color.G : color.B);
                if ((component & mask) != 0)
                    index |= (1 << (2 - i));
            }
            if (Children[index] == null)
            {
                Children[index] = new OctreeNode
                {
                    Depth = Depth + 1,
                    Parent = this,
                    ChildIndex = index,
                };
            }
            Children[index].Insert(color);

            RSum += color.R;
            GSum += color.G;
            BSum += color.B;
            Count += 1;
        }

        public int LeafCount
        {
            get
            {
                if (Children.All(c => c == null))
                {
                    return 1;
                }
                else
                {
                    return Children.Sum(c => c?.LeafCount ?? 0);
                }
            }
        }

        public void MergeLeastSignificant()
        {
            var leaves = GetLeaves();
            if (leaves.Count == 0)
                return;

            var nodeToMerge = leaves.OrderBy(n => n.Count).FirstOrDefault();
            if (nodeToMerge != null)
            {
                nodeToMerge.MergeWithParent();
            }
        }

        public List<OctreeNode> GetLeaves()
        {
            List<OctreeNode> leaves = new List<OctreeNode>();
            if (Children.All(c => c == null))
            {
                leaves.Add(this);
                return leaves;
            }
            foreach (var child in Children)
            {
                if (child != null)
                    leaves.AddRange(child.GetLeaves());
            }
            return leaves;
        }

        // 合并到父节点
        private void MergeWithParent()
        {
            // 将当前节点的颜色总和和计数合并到父节点
            Parent.RSum += RSum;
            Parent.GSum += GSum;
            Parent.BSum += BSum;
            Parent.Count += Count;

            // 删除当前节点
            Parent.Children[ChildIndex] = null;
        }

        public List<Color> GetPalette()
        {
            var palette = new List<Color>();
            CollectLeavesColors(this, palette);
            return palette;
        }

        private void CollectLeavesColors(OctreeNode node, List<Color> palette)
        {
            if (node.Children.All(c => c == null))
            {
                palette.Add(node.GetAverageColor());
                return;
            }
            foreach (var child in node.Children)
            {
                if (child != null)
                    CollectLeavesColors(child, palette);
            }
        }

        public int GetColorCount(Color targetColor)
        {
            var leaves = GetLeaves();
            if (leaves.Count == 0)
                return 0;

            var closestNode = leaves
                .OrderBy(leaf => ColorDistance(leaf.GetAverageColor(), targetColor))
                .FirstOrDefault();

            return closestNode?.Count ?? 0;
        }

        private double ColorDistance(Color c1, Color c2)
        {
            return Math.Sqrt(
                Math.Pow(c1.R - c2.R, 2) + Math.Pow(c1.G - c2.G, 2) + Math.Pow(c1.B - c2.B, 2)
            );
        }
    }
}
