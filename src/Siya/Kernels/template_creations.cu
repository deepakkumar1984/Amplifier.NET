__kernel void <DTYPE_NAME>_full(double fill_value, __global <DTYPE_NAME>* r)
{
    int i = get_global_id(0);
    r[i] = fill_value;
}