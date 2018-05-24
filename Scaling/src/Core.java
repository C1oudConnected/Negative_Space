import java.util.Scanner;

public class Core {
    public static void main(String[] args) {

        byte numSystem_A, numSystem_B;
        String a_numb;

        Scanner s = new Scanner(System.in);
        System.out.print("Enter the first numeral system: "); numSystem_A = Byte.parseByte(s.next());
        System.out.print("Enter the second numeral system: "); numSystem_B = Byte.parseByte(s.next());
        System.out.println("==================\n\nEnter the " + numSystem_A + "-al(ry) number: "); a_numb = s.next();

        double dec = 0;


        int c = ((a_numb.indexOf(',') != -1) ? a_numb.indexOf(',') : a_numb.length()) - 1;
        a_numb = a_numb.replace(",","");

        for (int i = 0; i < a_numb.length(); i++) {
            char a = a_numb.charAt(i);
            dec += ((a >= '0' && a <= '9') ? (a - '0') : (a - 'A' + 10)) * Math.pow(numSystem_A, c - i);
        } a_numb = "";


        int dec_int = (int)Math.floor(dec);
        dec -= dec_int;

        while (dec_int > 0) {
            int b;
            b = (dec_int) % numSystem_B;
            a_numb = (char)((b < 10) ? ('0' + b) : ('A' + b - 10)) + a_numb;
            dec_int /= numSystem_B;
        } a_numb += ',';


        for (int i = 0; i < 7; i++) {
            dec *= numSystem_B;
            double b = Math.floor(dec);
            a_numb += (char)((b >= 0 && b < 10) ? ('0' + b) : ('A' + b - 10));
            dec -= Math.floor(dec);
        }

        System.out.println("\n" + numSystem_B + "-al(ry) result:\n" + a_numb);
    }
}
