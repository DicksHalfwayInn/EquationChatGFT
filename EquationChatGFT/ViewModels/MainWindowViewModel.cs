using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;





namespace EquationChatGFT
{
    public partial class MainWindowViewModel : ObservableObject
    {

        public string Greeting => "Welcome to Avalonia!";

        private static readonly Regex _regex = new Regex(@"(\d+\.?\d*)|([\+\-\*\/])|(\()|(\))", RegexOptions.Compiled);
        private static readonly Dictionary<string, int> _precedence = new Dictionary<string, int>
        {
            ["*"] = 3,
            ["/"] = 3,
            ["+"] = 2,
            ["-"] = 2,
            ["("] = 1
        };

        //public static decimal Solve(string equation)
        //{
        //    var tokens = Tokenize(equation);
        //    var postfix = ToPostfix(tokens);
        //    return EvaluatePostfix(postfix);
        //}

        private string _equation = "1+1";
        public string Equation
        {
            get => _equation;
            set => SetProperty(ref _equation, value);
        }

        private decimal _result;
        public decimal Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public ICommand SolveCommand { get; }

        public MainWindowViewModel()
        {
            SolveCommand = new RelayCommand(OnSolve);
        }

        private void OnSolve()
        {
            var tokens = Tokenize(Equation);
            var postfix = ToPostfix(tokens);
            Result = EvaluatePostfix(postfix);
        }

        private static IEnumerable<string> Tokenize(string equation)
        {
            var tokens = new List<string>();
            var matches = _regex.Matches(equation);
            foreach (Match match in matches)
            {
                tokens.Add(match.Value);
            }
            return tokens;
        }

        private static IEnumerable<string> ToPostfix(IEnumerable<string> infix)
        {
            var stack = new Stack<string>();
            var postfix = new List<string>();

            foreach (var token in infix)
            {
                if (decimal.TryParse(token, out _))
                {
                    postfix.Add(token);
                }
                else if (token == "(")
                {
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    while (stack.Peek() != "(")
                    {
                        postfix.Add(stack.Pop());
                    }
                    stack.Pop();
                }
                else
                {
                    while (stack.Count > 0 && _precedence[stack.Peek()] >= _precedence[token])
                    {
                        postfix.Add(stack.Pop());
                    }
                    stack.Push(token);
                }
            }

            while (stack.Count > 0)
            {
                postfix.Add(stack.Pop());
            }

            return postfix;
        }

        private static decimal EvaluatePostfix(IEnumerable<string> postfix)
        {
            var stack = new Stack<decimal>();

            foreach (var token in postfix)
            {
                if (decimal.TryParse(token, out var value))
                {
                    stack.Push(value);
                }
                else
                {
                    var right = stack.Pop();
                    var left = stack.Pop();
                    switch (token)
                    {
                        case "+":
                            stack.Push(left + right);
                            break;
                        case "-":
                            stack.Push(left - right);
                            break;
                        case "*":
                            stack.Push(left * right);
                            break;
                        case "/":
                            stack.Push(left / right);
                            break;
                    }
                }
            }

            return stack.Pop();
        }
    }

}