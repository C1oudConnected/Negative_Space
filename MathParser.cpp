// MathParser.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <iostream>
#include <string>
#include "math.h"

// BinaryTree class;
class Tree {
	std::string value;
	Tree* left;
	Tree* right;
public:
	Tree(std::string _value, Tree* _left = NULL, Tree* _right = NULL)
		: value(_value), left(_left), right(_right) {};
	~Tree() {
		if (value == "+" || value == "-" || value == "*" || value == "/" || value == "^") {
			delete(left);
			delete(right);
		}
	}

	double evaluate(); //Evaluates the expression's value;
	
	void representInPrefix();
	void representInPostfix();
	void representInInfix();


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

void Tree::representInPrefix() {
	if (left == NULL)
		std::cout << value << " ";
	else {
		std::cout << value << " ";
		left->representInPrefix();
		right->representInPrefix();
	}
}

void Tree::representInPostfix() {
	if (left == NULL)
		std::cout << value << " ";
	else {
		right->representInPostfix();
		left->representInPostfix();
		std::cout << value << " ";
	}
}

void Tree::representInInfix() {
	if (left == NULL)
		std::cout << value << " ";
	else {
		if (((value == "*" || value == "/") && (left->value == "+" || left->value == "-")) || ((value == "^") && (left->value == "+" || left->value == "-" || left->value == "*" || left->value == "/")))
			std::cout << "( ";
		left->representInInfix();
		if (((value == "*" || value == "/") && (left->value == "+" || left->value == "-")) || ((value == "^") && (left->value == "+" || left->value == "-" || left->value == "*" || left->value == "/")))
			std::cout << ") ";
		std::cout << value << " ";
		if (((value == "*" || value == "/") && (right->value == "+" || right->value == "-")) || ((value == "^") && (right->value == "+" || right->value == "-" || right->value == "*" || right->value == "/")))
			std::cout << "( ";
		right->representInInfix();
		if (((value == "*" || value == "/") && (right->value == "+" || right->value == "-")) || ((value == "^") && (right->value == "+" || right->value == "-" || right->value == "*" || right->value == "/")))
			std::cout << ") ";
	}
}


// Main parsing function;

Tree* parseStringToTree(char* string) {
	static int i; //Symbol counter;
	Tree* ret_val = NULL; //Return value;
	int numb_start = -1; // start position of a current number;
	std::string val;	 // current number string;
	bool prev_brackets = false; // If previous expression was in brackets;

	for (; true; i++) { // Main loop;

		// Check if number starts;
		if ((string[i] >= '0' && string[i] <= '9') || string[i] == '.') {
			if (numb_start == -1) numb_start = i;
		}
		else {

			//If number is started and finished, store it to the string;
			if (numb_start != -1) {
				val = "";
				for (int j = 0; j < i - numb_start; j++)
					val += string[numb_start + j];

				// Make a Tree Node with a current number;
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

				// Exit if expression ends;

				if (string[i] == '\0') {
					i = 0;
					break;
				}

				if (string[i] == ')') {
					i++;
					break;
				}
			}
			// Recursively process the expression in brackets;
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

				//Exit if expression ends;

				if (string[i] == '\0') {
					i = 0;
					break;
				}

				if (string[i] == ')') {
					i++;
					break;
				}
			}

			// Process + and -;
			if (string[i] == '+' || string[i] == '-') {
				std::string temp = "", q = "?";
				temp += string[i];
				ret_val = new Tree(temp, ret_val, new Tree(q));
				prev_brackets = false;
				numb_start = -1;
			}

			// Process * and /;
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

			// Process ^;
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
	std::string str = "(2+3)*2^(4*(2+4))+0.5";
	Tree* t = parseStringToTree((char*)str.data());

	std::cout << str;
	std::cout << std::endl <<"Value = " << t->evaluate() << std::endl;
	std::cout << "Representation in prefix: "; t->representInPrefix();
	std::cout << std::endl;
	std::cout << "Representation in postfix: "; t->representInPostfix();
	std::cout << std::endl;
	std::cout << "Representation in infix: "; t->representInInfix();
	std::cout << std::endl;
	delete(t);
    return 0;
}


