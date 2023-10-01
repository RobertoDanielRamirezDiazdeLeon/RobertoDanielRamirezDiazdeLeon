#include <stdio.h>

int altura,i,j;
float x,y;

void main()
{
    y /= 10;
    x += (3 + 5) * 8 - (10 - 4) / 2; // = 61
    
    printf("\nValor de altura = ");
    scanf("%i",&altura);
    print(altura);
    for (i = 1; i <= altura; i++)
    {
        if (i == 5)
        {
            j = 1;
            y--;
        }
        else if (i == 0)
            j = 2;
        else
            j = 3;
        while (j <= i)
        { 
            printf("%f",j);
        }
        printf("\n");
    }
    i = 0;
    do
    {
        printf("Hola")
        if (i%2==0)
            printf("-");
        else
            i*=(altura+2);
    }
    while (i<altura);
}
