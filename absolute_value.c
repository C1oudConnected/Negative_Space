int absolute_value(int v) {
	int lastBit = 1;
    while (lastBit<<1)
		lastBit <<= 1;
    return v * ((v & lastBit)/lastBit * (-2) + 1);	
}

// If there is sizeof();

int absolute_value_C(int v) {
    return v * (v & 1<<(sizeof(v*8)-1))/(1<<(sizeof(v*8)-1)) * (-2) + 1);	
}
5
