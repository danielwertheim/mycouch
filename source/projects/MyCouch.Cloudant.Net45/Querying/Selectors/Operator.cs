using EnsureThat;
using MyCouch.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCouch.Cloudant.Querying.Selectors
{
    public abstract class Operation
    {
        internal abstract string ToJson(ISerializer serializer);
    }

    public class ComparisonOperation : Operation
    {
        private readonly string _propertyExpression;
        private readonly object _operand;
        private readonly ComparisonOperator _type;
        internal ComparisonOperation(string propertyExpression, ComparisonOperator type, object operand)
        {
            _type = type;
            _propertyExpression = propertyExpression;
            _operand = operand;
        }

        internal override string ToJson(ISerializer serializer)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat(FormatStrings.JsonPropertyFormat, _propertyExpression, GetOperandJson(serializer));
            sb.Append("}");

            return sb.ToString();
        }

        private string GetOperandJson(ISerializer serializer)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat(FormatStrings.JsonPropertyFormat, _type.AsString(), serializer.ToJson(_operand));
            sb.Append("}");

            return sb.ToString();
        }
    }

    public class CombinationOperation : Operation
    {
        internal readonly IList<Operation> _operations;
        private readonly CombinationOperator _type;
        internal CombinationOperation(CombinationOperator type)
        {
            _type = type;
            _operations = new List<Operation>();
        }

        internal IList<Operation> Operations { get { return _operations; } }

        internal void AddOperation(Operation operation)
        {
            _operations.Add(operation);
        }

        internal override string ToJson(ISerializer serializer)
        {
            var sb = new StringBuilder();
            sb.Append("{");

            sb.AppendFormat(FormatStrings.JsonPropertyFormat, _type.AsString(), GetJsonArrayForChildren(serializer));

            sb.Append("}");
            return sb.ToString();
        }

        private string GetJsonArrayForChildren(ISerializer serializer)
        {
            Ensure.That(_operations, "operations").HasItems();
            var sb = new StringBuilder();
            sb.Append("[");

            for (int i = 0; i < _operations.Count; i++)
                sb.AppendFormat("{0},", _operations[i].ToJson(serializer));
            sb.Remove(sb.Length - 1, 1);

            sb.Append("]");
            return sb.ToString();
        }
    }

    public enum ComparisonOperator
    {
        GreaterThan,
    }

    public enum CombinationOperator
    {
        And,
    }

    public static class OperatorExtensions
    {
        private static readonly Dictionary<ComparisonOperator, string> ComparisonMappings;
        private static readonly Dictionary<CombinationOperator, string> CombinationMappings;

        static OperatorExtensions()
        {
            ComparisonMappings = new Dictionary<ComparisonOperator, string>
            {
                { ComparisonOperator.GreaterThan, "$gt" },
            };
            CombinationMappings = new Dictionary<CombinationOperator, string>
            {
                { CombinationOperator.And, "$and" },
            };
        }

        public static string AsString(this ComparisonOperator comparisonOperator)
        {
            return ComparisonMappings[comparisonOperator];
        }

        public static string AsString(this CombinationOperator combinationOperator)
        {
            return CombinationMappings[combinationOperator];
        }
    }

    public class Selector
    {
        private readonly CombinationOperation _root;
        public Selector() : this(CombinationOperator.And) { }

        public Selector(CombinationOperator combinationType)
        {
            _root = new CombinationOperation(combinationType);
        }

        public Selector Configure(Action<SelectorConfigurator> configurator)
        {
            configurator(new SelectorConfigurator(this));
            return this;
        }

        internal void AddComparison(string propertyExpression, ComparisonOperator operatorType, object operand)
        {
            _root.AddOperation(new ComparisonOperation(propertyExpression, operatorType, operand));
        }

        internal string ToJson(ISerializer serializer)
        {
            return _root.ToJson(serializer);
        }
    }

    public class SelectorConfigurator
    {
        private readonly Selector _selectorToConfigure;

        internal SelectorConfigurator(Selector selectorToConfigure)
        {
            _selectorToConfigure = selectorToConfigure;
        }
        public SelectorConfigurator AddComparison(string propertyExpression, ComparisonOperator operatorType, object operand)
        {
            _selectorToConfigure.AddComparison(propertyExpression, operatorType, operand);
            return this;
        }
    }
}
