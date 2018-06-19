// If there is sizeof();
T - integer type of unknown size

T absolute_value_C(T v) {
	// with Sign Extension
	return v ^ (v>>(sizeof(v)<<3)) + v>>(sizeof(v)<<3);
	
	
	// without Sign Extension and sizeof() (unfinished)
	return v * (v & (-1<<1))/(-1<<1) * (-2) + 1;
}
