using System;
using System.Collections.Generic;
using System.Text;

namespace CardDrawTest
{
    public class RandomDraw
    {
        /// <summary>
        /// 把抽奖概率精准到0.01%
        /// </summary>
        public const uint Parameters = 100;
        private readonly List<string> _name = new List<string>();
        private readonly List<uint> _probability = new List<uint>();
        private static readonly Random _random = new Random(Guid.NewGuid().GetHashCode());       

        public void AddPrize(string name, decimal probability)
        {
            if (probability <= 0.009999999M)
            {
                throw new ArgumentOutOfRangeException();
            }
            _name.Add(name);
            _probability.Add((uint)(probability*Parameters));
        }

        public string GetRandomResult()
        {
            var result = GetResult(_probability);
            return _name[result];
        }

        private static int GetResult(List<uint> array)
        {
            uint totalNumber = GetTotalNumber(array);
            
            double random;
            for (var i = 0; i < array.Count; ++i)
            {
                //获取 0-总数 之间的一个随随机整数
                random = _random.NextDouble() * totalNumber;
                if (random < array[i])
                {
                    //如果在当前的概率范围内,得到的就是当前概率
                    return i;
                }
                else
                {
                    //否则减去当前的概率范围,进入下一轮循环
                    totalNumber -= array[i];
                }
            }
            return 0;
        }

        private static uint GetTotalNumber(List<uint> list)
        {
            uint totalNumber = 0;
            list.ForEach((uint i) => totalNumber += i);
            return totalNumber;
        }

        public override int GetHashCode()
        {
            int result = GetNameHashCode() * 31 + GetProbabilityHashCode();
            return result;
        }

        private int GetNameHashCode()
        {
            var sb = new StringBuilder();
            foreach (var s in _name)
            {
                sb.Append(s);
            }
            return sb.ToString().GetHashCode();
        }

        private int GetProbabilityHashCode()
        {
            int result = 235320395;
            foreach (var num in _probability)
            {
                result = 31 * result + num.GetHashCode();
            }
            return result;
        }

        public override string ToString()
        {
            if (_name.Count == 0)
                return "[]";
            var sb = new StringBuilder("[");
            for (int i = 0, max = _name.Count - 1; i < max; ++i)
            {
                sb.Append($"{_name[i]}={_probability[i]}, ");
            }
            sb.Append($"{_name[_name.Count - 1]}={_probability[_probability.Count - 1]}]");
            return sb.ToString();
        }
    }
}
