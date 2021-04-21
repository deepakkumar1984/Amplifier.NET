using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siya
{
    public partial class sx
    {
        public static NDArray abs(NDArray x) => unary_exec(x, "abs");

        public static NDArray acos(NDArray x) => unary_exec(x, "acos");

        public static NDArray acosh(NDArray x) => unary_exec(x, "acosh");

        public static NDArray add(NDArray x1, NDArray x2) => binary_exec(x1, x2, "add");

        public static NDArray asin(NDArray x) => unary_exec(x, "asin");

        public static NDArray asinh(NDArray x) => unary_exec(x, "asinh");

        public static NDArray atan(NDArray x) => unary_exec(x, "atan");

        public static NDArray atan2(NDArray y, NDArray x) => binary_exec(y, x, "atan2");

        public static NDArray atanh(NDArray x) => unary_exec(x, "atanh");

        public static NDArray bitwise_and(NDArray x1, NDArray x2) => binary_exec(x1, x2, "bitwise_and");

        public static NDArray bitwise_or(NDArray x1, NDArray x2) => binary_exec(x1, x2, "bitwise_or");

        public static NDArray bitwise_xor(NDArray x1, NDArray x2) => binary_exec(x1, x2, "bitwise_xor");

        public static NDArray bitwise_left_shift(NDArray x1, NDArray x2) => binary_exec(x1, x2, "left_shift");

        public static NDArray bitwise_right_shift(NDArray x1, NDArray x2) => binary_exec(x1, x2, "right_shift");

        public static NDArray bitwise_invert(NDArray x) => throw new NotImplementedException();

        public static NDArray ceil(NDArray x) => unary_exec(x, "ceil");

        public static NDArray cos(NDArray x) => unary_exec(x, "cos");

        public static NDArray cosh(NDArray x) => unary_exec(x, "cosh");

        public static NDArray equal(NDArray x1, NDArray x2) => binary_exec(x1, x2, "eq");

        public static NDArray exp(NDArray x) => unary_exec(x, "exp");

        public static NDArray expm1(NDArray x) => unary_exec(x, "expm1");

        public static NDArray floor(NDArray x) => unary_exec(x, "floor");

        public static NDArray floor_divide(NDArray x1, NDArray x2) => binary_exec(x1, x2, "floor_divide");

        public static NDArray greater(NDArray x1, NDArray x2) => binary_exec(x1, x2, "gt");

        public static NDArray greater_equal(NDArray x1, NDArray x2) => binary_exec(x1, x2, "ge");

        public static NDArray less(NDArray x1, NDArray x2) => binary_exec(x1, x2, "lt");

        public static NDArray less_equal(NDArray x1, NDArray x2) => binary_exec(x1, x2, "le");

        public static NDArray isfinite(NDArray x) => unary_exec(x, "isfinite");

        public static NDArray isinf(NDArray x) => unary_exec(x, "isinf");

        public static NDArray isnan(NDArray x) => unary_exec(x, "isnan");

        public static NDArray log(NDArray x) => unary_exec(x, "log");

        public static NDArray log1p(NDArray x) => unary_exec(x, "log1p");

        public static NDArray log2(NDArray x) => unary_exec(x, "log2");

        public static NDArray log10(NDArray x) => unary_exec(x, "log10");

        public static NDArray logaddexp(NDArray x1, NDArray x2) => binary_exec(x1, x2, "logaddexp");

        public static NDArray logical_and(NDArray x1, NDArray x2) => binary_exec(x1, x2, "logical_and");

        public static NDArray logical_not(NDArray x1, NDArray x2) => binary_exec(x1, x2, "logical_not");

        public static NDArray logical_or(NDArray x1, NDArray x2) => binary_exec(x1, x2, "logical_or");

        public static NDArray logical_xor(NDArray x1, NDArray x2) => throw new NotImplementedException();

        public static NDArray multiply(NDArray x1, NDArray x2) => binary_exec(x1, x2, "multiply");

        public static NDArray negative(NDArray x) => unary_exec(x, "negative");

        public static NDArray not_equal(NDArray x1, NDArray x2) => binary_exec(x1, x2, "noteq");

        public static NDArray positive(NDArray x) => unary_exec(x, "positive");

        public static NDArray power(NDArray x1, NDArray x2) => binary_exec(x1, x2, "power");

        public static NDArray remainder(NDArray x1, NDArray x2) => binary_exec(x1, x2, "remainder");

        public static NDArray round(NDArray x) => unary_exec(x, "round");

        public static NDArray sign(NDArray x) => unary_exec(x, "sign");

        public static NDArray sinh(NDArray x) => unary_exec(x, "sinh");

        public static NDArray sin(NDArray x) => unary_exec(x, "sin");

        public static NDArray square(NDArray x) => unary_exec(x, "square");

        public static NDArray sqrt(NDArray x) => unary_exec(x, "sqrt");

        public static NDArray subtract(NDArray x1, NDArray x2) => binary_exec(x1, x2, "subtract");

        public static NDArray tan(NDArray x) => unary_exec(x, "tan");

        public static NDArray tanh(NDArray x) => unary_exec(x, "tanh");

        public static NDArray trunc(NDArray x) => unary_exec(x, "trunc");
    }
}
