// If there is sizeof();
T - integer type of unknown size

T absolute_value_C(T v) {
	// with Sign Extension
	return v ^ (v>>(sizeof(v)>>3)) + v>>(sizeof(v)>>3);
	
	
	// without Sign Extension (unfinished)
	return v & (v & 1<<(sizeof(v)*8-1))/(1<<(sizeof(v)*8-1)) * (-2) + 1);	
}
