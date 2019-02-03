// MathParser.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include "math.h"

class Tree {
	std::string value;
	Tree* left;
	Tree* right;
public:
	Tree(std::string _value, Tree* _left = NULL, Tree* _right = NULL)
		: value(_value), left(_left), right(_right) {};

	double evaluate();
	friend Tree* parseStringToTree(char*);
};

double Tree::evaluate() {
	if (this->left == NULL)
		return std::stod(value);
	else 
		if (value == "+") return left->evaluate() + right->evaluate();
		else if (value == "-") return left->evaluate() - right->evaluate();
		else if (value == "*") return left->evaluate() * right->evaluate();
		else if (value == "/") return left->evaluate() / right->evaluate();
		else if (value == "^") return pow(left->evaluate(), right->evaluate());
}

Tree* parseStringToTree(char* string) {
	static int i = 0;
	Tree* ret_val = NULL;
	int numb_start = -1;
	std::string val;
	bool prev_brackets = false;

	for (; true; i++) {

		if ((string[i] >= '0' && string[i] <= '9') || string[i] == '.') {
			if (numb_start == -1) numb_start = i;
		}
		else {

			if (numb_start != -1) {
				val = "";

				for (int j = 0; j < i - numb_start; j++)
					val += string[numb_start + j];

				Tree* temp = new Tree(val);
				if (ret_val == NULL)
					ret_val = temp;
				else if (ret_val->right->value == "?") {
					delete ret_val->right;
					ret_val->right = temp;
				}
				else {
					delete ret_val->right->right;
					ret_val->right->right = temp;
				}
				if (string[i] == '\0' || string[i] == ')') {
					i++;
					return ret_val;
				}
			}
			if (string[i] == '(') {
				i++;
				Tree* temp = parseStringToTree(string);
				if (ret_val == NULL)
					ret_val = temp;
				else if (ret_val->right->value == "?") {
					delete ret_val->right;
					ret_val->right = temp;
				}
				else if (ret_val->right->right->value == "?") {
					delete ret_val->right->right;
					ret_val->right->right = temp;
				}
				prev_brackets = true;
				numb_start = -1;
				if (string[i] == '\0' || string[i] == ')')
					return ret_val;
			}

			if (string[i] == '+' || string[i] == '-') {
				std::string temp = "", q = "?";
				temp += string[i];
				ret_val = new Tree(temp, ret_val, new Tree(q));
				prev_brackets = false;
				numb_start = -1;
			}
			if (string[i] == '*' || string[i] == '/') {
				if (prev_brackets || ret_val->left == NULL) {
					std::string temp = "", q = "?";
					temp += string[i];
					ret_val = new Tree(temp, ret_val, new Tree(q));
				}
				else {
					std::string temp = "", q = "?";
					temp += string[i];
					ret_val->right = new Tree(temp, ret_val->right, new Tree(q));
				}
				prev_brackets = false;
				numb_start = -1;
			}
			if (string[i] == '^') {

				std::string temp = "", q = "?";
				temp += string[i];
				if (prev_brackets || ret_val->left == NULL) {
					ret_val = new Tree(temp, ret_val, new Tree(q));
				}
				else {
					
					if (ret_val->right->right == NULL)
						ret_val->right = new Tree(temp, ret_val->right, new Tree(q));
					else 
						ret_val->right->right = new Tree(temp, ret_val->right, new Tree(q));
				
				}
				prev_brackets = false;
				numb_start = -1;
			}
		}

	}
	return ret_val;
}

int main()
{
	Tree* t = parseStringToTree((char*)"5.2*10^2");
	std::cout << "(2+3)*2^(4*(2+4))";
	std::cout << std::endl<<t->evaluate() << std::endl;
    return 0;
}

