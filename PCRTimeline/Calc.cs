using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    class Calc
    {
        enum Operator
        {
            Unknown,
            Number,
            Alphabet,
            Plus,
            Minus,
            Multi,
            Divide,
            Mod,
            LParen,
            RParen,
            Camma,
            Space,
        }

        class Lexical
        {
            public Operator op;
            public string str;

            public override string ToString() { return $"op={op}, str={str}"; }
        }

        List<Lexical> unitlist = new List<Lexical>();
        int current;

        public interface NodeBase
        {
            float Calc(Func<string, List<float>, float> func);
            bool IsCalcable { get; }
        }

        class NodeNumber : NodeBase
        {
            float value;

            public bool IsCalcable => true;

            public float Calc(Func<string, List<float>, float> func)
            {
                return value;
            }

            public NodeNumber(float value)
            {
                this.value = value;
            }
        }

        class NodeNegative : NodeBase
        {
            NodeBase node;
            public bool IsCalcable => node.IsCalcable;

            public float Calc(Func<string, List<float>, float> func)
            {
                return -node.Calc(func);
            }

            public NodeNegative(NodeBase node)
            {
                this.node = node;
            }
        }

        class NodeFunction : NodeBase
        {
            string funcname;
            List<NodeBase> nodearg = new List<NodeBase>();

            public NodeFunction(string func, List<NodeBase> nodearg)
            {
                this.funcname = func;
                this.nodearg = nodearg;
            }

            public float Calc(Func<string, List<float>, float> func)
            {
                List<float> numberarg = new List<float>();

                if (nodearg != null)
                {
                    foreach (var item in nodearg)
                    {
                        numberarg.Add(item.Calc(func));
                    }
                }

                return func(funcname, numberarg);
            }
            public bool IsCalcable => false;
        }

        class NodeTree : NodeBase
        {
            private Operator op;
            private NodeBase term1;
            private NodeBase term2;

            public NodeTree(Operator op, Calc.NodeBase term1, Calc.NodeBase term2)
            {
                this.op = op;
                this.term1 = term1;
                this.term2 = term2;
            }

            public bool IsCalcable => (term1 == null ? term1.IsCalcable : true) && (term2 == null ? term2.IsCalcable : true);

            public float Calc(Func<string, List<float>, float> func)
            {
                switch (op)
                {
                    case Operator.Plus:
                        return term1.Calc(func) + term2.Calc(func);
                    case Operator.Minus:
                        return term1.Calc(func) - term2.Calc(func);
                    case Operator.Multi:
                        return term1.Calc(func) * term2.Calc(func);
                    case Operator.Divide:
                        return term1.Calc(func) / term2.Calc(func);
                    case Operator.Mod:
                        return term1.Calc(func) % term2.Calc(func);
                }
                return 0;
            }
        }

        public NodeBase Calcrator(string formula)
        {
            LexicalAnalysis(formula);

            current = 0;
            return GetExpr();
        }

        NodeBase GetExpr()
        {
            var term1 = GetTerm();

            var next = GetNext();
            if (next == null) return term1;
            if (next.op == Operator.Plus || next.op == Operator.Minus)
            {
                var term2 = GetTerm();
                return new NodeTree(next.op, term1, term2);
            }
            Unget();
            return term1;
        }

        NodeBase GetTerm()
        {
            var term1 = GetFactor();

            var next = GetNext();
            if (next == null) return term1;
            if (next.op == Operator.Multi || next.op == Operator.Divide || next.op == Operator.Mod)
            {
                var term2 = GetFactor();
                return new NodeTree(next.op, term1, term2);
            }
            Unget();
            return term1;
        }

        NodeBase GetFactor()
        {
            var lparen = GetNext();
            if (lparen == null) return null;

            switch (lparen.op)
            {
                case Operator.LParen:
                    {
                        var expr = GetExpr();
                        var rparen = GetNext();
                        if (rparen == null || rparen.op == Operator.RParen) return expr;
                        Unget();
                        break;
                    }
                default:
                    Unget();
                    return GetNumber();
            }
            return null;
        }

        NodeBase GetNumber()
        {
            var unit = GetNext();

            switch (unit.op)
            {
                case Operator.Number:
                    return new NodeNumber(float.Parse(unit.str));
                case Operator.Alphabet:
                    Unget();
                    return GetFunction();
                case Operator.Plus:
                    return GetNumber();
                case Operator.Minus:
                    {
                        var minus = GetNumber();
                        return minus != null ? new NodeNegative(minus) : null;
                    }
            }
            return null;
        }

        private NodeBase GetFunction()
        {
            var funcname = GetNext();
            if (funcname == null || funcname.op != Operator.Alphabet) return null;

            var lparen = GetNext();
            if (lparen == null || lparen.op != Operator.LParen)
            {
                Unget();
                return new NodeFunction(funcname.str, null);
            }

            var arg = new List<NodeBase>();
            for (; ; )
            {
                var node = GetExpr();
                if (node == null) new NodeFunction(funcname.str, arg);
                arg.Add(node);

                var end = GetNext();

                if (end == null || end.op == Operator.RParen)
                {
                    return new NodeFunction(funcname.str, arg);
                }
                if (end.op != Operator.Camma)
                {
                    return null;
                }
            }

        }

        Lexical GetNext()
        {
            if (current < unitlist.Count)
            {
                return unitlist[current++];
            }
            return null;
        }

        void Unget()
        {
            current--;
        }

        void LexicalAnalysis(string formula)
        {
            unitlist.Clear();

            for (int idx = 0; idx < formula.Length; idx++)
            {
                char c = formula[idx];
                Operator op = GetOperator(c);

                if (op == Operator.Space) continue;
                if (op == Operator.Unknown) throw new Exception($"unknown letter: {c}");

                if (op == Operator.Number)
                {
                    var str = GetContinuity(formula, n => n == Operator.Number, ref idx);
                    unitlist.Add(new Lexical() { op = op, str = str });
                }
                else if (op == Operator.Alphabet)
                {
                    var str = GetContinuity(formula, n => n == Operator.Number || n == Operator.Alphabet, ref idx);
                    unitlist.Add(new Lexical() { op = op, str = str });

                }
                else
                {
                    unitlist.Add(new Lexical() { op = op });
                }

            }
        }

        private string GetContinuity(string formula, Func<Operator, bool> compare, ref int idx)
        {
            int start = idx;
            for (int i = idx + 1; i < formula.Length; i++)
            {
                if (!compare(GetOperator(formula[i])))
                {
                    idx = i - 1;
                    return formula.Substring(start, i - start);
                }
            }
            idx = formula.Length - 1;
            return formula.Substring(start);
        }

        private Operator GetOperator(char c)
        {
            switch (c)
            {
                case '+': return Operator.Plus;
                case '-': return Operator.Minus;
                case '*': return Operator.Multi;
                case '/': return Operator.Divide;
                case '%': return Operator.Mod;
                case '(': return Operator.LParen;
                case ')': return Operator.RParen;
                case ',': return Operator.Camma;
                case ' ': return Operator.Space;
                case '\t': return Operator.Space;
                case '.': return Operator.Number;
            }

            if (Char.IsNumber(c)) return Operator.Number;
            if (Char.IsLetter(c)) return Operator.Alphabet;

            return Operator.Unknown;
        }
    }

}

