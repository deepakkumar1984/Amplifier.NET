__kernel void <DTYPE_NAME>_matmul(const long M, const long N, const long K,
                      const __global <DTYPE_NAME>* A,
                      const __global <DTYPE_NAME>* B,
                      __global <DTYPE_NAME>* C) {
    
    // Thread identifiers
    const int globalRow = get_global_id(0); // Row ID of C (0..M)
    const int globalCol = get_global_id(1); // Col ID of C (0..N)
    
    // Compute a single element (loop over K)
    <DTYPE_NAME> acc = 0;
    for (int k=0; k<K; k++) {
        acc += A[k*M + globalRow] * B[globalCol*K + k];
    }
 
    // Store the result
    C[globalCol*M + globalRow] = acc;
}