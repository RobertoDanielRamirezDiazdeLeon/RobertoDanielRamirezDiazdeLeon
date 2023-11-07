; Autor: Roberto Daniel Ramírez Díaz de León
; 25/10/2023 02:16:44 p. m.
include 'emu8086.inc'
org 100h
PRINT ''
PRINTN ' '
PRINT 'dame un numero: '
CALL SCAN_NUM
MOV altura, CX
PRINTN ' '
; if: 1
MOV AX, altura
PUSH AX
MOV AX, 3
PUSH AX
POP BX
POP AX
CMP AX, BX
JNE Eif1
PRINT 'Hola'
; else: 1
JMP Eelse1
Eif1:
; if: 2
MOV AX, altura
PUSH AX
MOV AX, 5
PUSH AX
POP BX
POP AX
CMP AX, BX
JNE Eif2
PRINT '5'
; else: 2
JMP Eelse2
Eif2:
PRINT 'Cualquier cosa'
Eelse2:
Eelse1:
int 20h
RET
define_scan_num
define_clear_screen
define_print_num
define_print_num_uns
; V a r i a b l e s
altura dw 0h 
i dw 0h 
j dw 0h 
k dw 0h 
m dw 0h 
p dw 0h 
END
