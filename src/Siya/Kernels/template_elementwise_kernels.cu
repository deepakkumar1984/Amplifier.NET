__kernel void <DTYPE_NAME>_add(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] + y[i];
}

__kernel void <DTYPE_NAME>_subtract(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] - y[i];
}

__kernel void <DTYPE_NAME>_multiply(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] * y[i];
}

__kernel void <DTYPE_NAME>_divide(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] / y[i];
}

__kernel void <DTYPE_NAME>_remainder(__global <DTYPE_NAME> *x1, __global <DTYPE_NAME> *x2, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = remainder(x1[i], x2[i]);
}

__kernel void <DTYPE_NAME>_gt(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    if(x[i] > y[i]){
        r[i] = 1;
    }
    else{
        r[i] = 0;
    }
}

__kernel void <DTYPE_NAME>_ge(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    if(x[i] >= y[i]){
        r[i] = 1;
    }
    else{
        r[i] = 0;
    }
}

__kernel void <DTYPE_NAME>_lt(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    if(x[i] < y[i]){
        r[i] = 1;
    }
    else{
        r[i] = 0;
    }
}

__kernel void <DTYPE_NAME>_le(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    if(x[i] <= y[i]){
        r[i] = 1;
    }
    else{
        r[i] = 0;
    }
}

__kernel void <DTYPE_NAME>_eq(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    if(x[i] == y[i]){
        r[i] = 1;
    }
    else{
        r[i] = 0;
    }
}

__kernel void <DTYPE_NAME>_noteq(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    if(x[i] != y[i]){
        r[i] = 1;
    }
    else{
        r[i] = 0;
    }
}

__kernel void <DTYPE_NAME>_bitwise_and(__global int *x, __global int *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] & y[i];
}

__kernel void <DTYPE_NAME>_bitwise_or(__global int *x, __global int *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] | y[i];
}

__kernel void <DTYPE_NAME>_bitwise_xor(__global int *x, __global int *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] ^ y[i];
}

__kernel void <DTYPE_NAME>_bitwise_not(__global int* x, __global <DTYPE_NAME>* r)
{
    int i = get_global_id(0);
    r[i] = ~x[i];
}

__kernel void <DTYPE_NAME>_logical_and(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] && y[i];
}

__kernel void <DTYPE_NAME>_logical_or(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] || y[i];
}

/*
__kernel void <DTYPE_NAME>_logical_xor(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] ^ y[i];
}
*/

__kernel void <DTYPE_NAME>_left_shift(__global int* x, __global int *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] << y[i];
}

__kernel void <DTYPE_NAME>_right_shift(__global int *x, __global int *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] >> y[i];
}

__kernel void <DTYPE_NAME>_floor_divide(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = floor(x[i] / y[i]);
}

__kernel void <DTYPE_NAME>_logaddexp(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *y, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = log(exp(x[i]) + exp(y[i]));
}

__kernel void <DTYPE_NAME>_abs(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = fabs(x[i]);
}

__kernel void <DTYPE_NAME>_acos(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = acos(x[i]);
}

__kernel void <DTYPE_NAME>_acosh(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = acosh(x[i]);
}

__kernel void <DTYPE_NAME>_asin(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = asin(x[i]);
}

__kernel void <DTYPE_NAME>_asinh(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = asinh(x[i]);
}

__kernel void <DTYPE_NAME>_atan(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = atan(x[i]);
}

__kernel void <DTYPE_NAME>_atan2(__global <DTYPE_NAME> *y, __global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = atan2(y[i], x[i]);
}

__kernel void <DTYPE_NAME>_atanh(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = atanh(x[i]);
}

__kernel void <DTYPE_NAME>_ceil(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = asinh(x[i]);
}

__kernel void <DTYPE_NAME>_cos(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = cos(x[i]);
}

__kernel void <DTYPE_NAME>_cosh(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = cosh(x[i]);
}

__kernel void <DTYPE_NAME>_exp(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = exp(x[i]);
}

__kernel void <DTYPE_NAME>_expm1(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = expm1(x[i]);
}

__kernel void <DTYPE_NAME>_floor(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = floor(x[i]);
}

__kernel void <DTYPE_NAME>_isfinite(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = isfinite(x[i]);
}

__kernel void <DTYPE_NAME>_isinf(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = isinf(x[i]);
}

__kernel void <DTYPE_NAME>_isnan(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = isnan(x[i]);
}

__kernel void <DTYPE_NAME>_log(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = log(x[i]);
}

__kernel void <DTYPE_NAME>_log1p(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = log1p(x[i]);
}

__kernel void <DTYPE_NAME>_log2(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = log2(x[i]);
}

__kernel void <DTYPE_NAME>_log10(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = log10(x[i]);
}

__kernel void <DTYPE_NAME>_negative(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = x[i] * -1;
}

__kernel void <DTYPE_NAME>_positive(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    if(x[i] < 0){
        r[i] = x[i] * -1;
    }
    else{
        r[i] = x[i];
    }
}

__kernel void <DTYPE_NAME>_power(__global <DTYPE_NAME> *x1, __global <DTYPE_NAME> *x2, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = pow(x1[i], x2[i]);
}

__kernel void <DTYPE_NAME>_round(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = round(x[i]);
}

__kernel void <DTYPE_NAME>_sign(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = sign(x[i]);
}

__kernel void <DTYPE_NAME>_sinh(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = sinh(x[i]);
}

__kernel void <DTYPE_NAME>_sin(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = sin(x[i]);
}

__kernel void <DTYPE_NAME>_square(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = pown(x[i], 2);
}

__kernel void <DTYPE_NAME>_sqrt(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = sqrt(x[i]);
}

__kernel void <DTYPE_NAME>_tan(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = tan(x[i]);
}

__kernel void <DTYPE_NAME>_tanh(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = tan(x[i]);
}

__kernel void <DTYPE_NAME>_trunc(__global <DTYPE_NAME> *x, __global <DTYPE_NAME> *r)
{
    int i = get_global_id(0);
    r[i] = trunc(x[i]);
}