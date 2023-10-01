#include<stdio.h>
#include<math.h>
#include<iostream>

char  a;
int   b,i;
float c;

void main() // Funcion principal
{
    c = 20;
    printf("C = ",c);
    a = (char)((char)(c) + (float)(b));

    if (1 == 2)
    {
        printf("Hola");
        if (1==1)
        {
            printf(" a todos");
        }
        else
        {
            printf(" a nadie");

            for (i=0; i<10; i++)
            {
                printf("Hola");
            }
        }
    }
    else
    {
        printf("mundo\n");
    }
}
