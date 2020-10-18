using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneUI.Utils
{
  public static class CalculateExtension
  {
    public static string ToOperator(this int op)
    {
      switch (op)
      {
        case 1:
          return "+ ";
        case 2:
          return "- ";
        case 3:
          return "* ";
        case 4:
          return "/ ";
      }
      return "+ ";
    }
  }

  public class CalculateUtil
  {
    public static decimal evaluate(string expression)
    {
      char[] tokens = expression.ToCharArray();

      Stack<decimal> values = new Stack<decimal>();
      Stack<char> ops = new Stack<char>();

      for (int i = 0; i < tokens.Length; i++)
      {
        if (tokens[i] == ' ')
        {
          continue;
        }

        if (tokens[i] >= '0' && tokens[i] <= '9')
        {
          StringBuilder sbuf = new StringBuilder();
          while (i < tokens.Length && ((tokens[i] >= '0' && tokens[i] <= '9') || tokens[i] == 46))
          {
            sbuf.Append(tokens[i++]);
          }
          values.Push(decimal.Parse(sbuf.ToString()));
        }

        else if (tokens[i] == '(')
        {
          ops.Push(tokens[i]);
        }

        else if (tokens[i] == ')')
        {
          while (ops.Peek() != '(')
          {
            values.Push(applyOp(ops.Pop(), values.Pop(), values.Pop()));
          }
          ops.Pop();
        }

        else if (tokens[i] == '+' || tokens[i] == '-' || tokens[i] == '*' || tokens[i] == '/')
        {
          while (ops.Count > 0 && hasPrecedence(tokens[i], ops.Peek()))
          {
            values.Push(applyOp(ops.Pop(), values.Pop(), values.Pop()));
          }

          ops.Push(tokens[i]);
        }
      }

      while (ops.Count > 0)
      {
        values.Push(applyOp(ops.Pop(), values.Pop(), values.Pop()));
      }

      return values.Pop();
    }

    public static bool hasPrecedence(char op1, char op2)
    {
      if (op2 == '(' || op2 == ')')
      {
        return false;
      }
      if ((op1 == '*' || op1 == '/') && (op2 == '+' || op2 == '-'))
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    public static decimal applyOp(char op, decimal b, decimal a)
    {
      switch (op)
      {
        case '+':
          return a + b;
        case '-':
          return a - b;
        case '*':
          return a * b;
        case '/':
          if (b == 0)
          {
            throw new System.NotSupportedException("Cannot divide by zero");
          }
          return a / b;
      }
      return 0;
    }

  }

}
