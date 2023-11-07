; Autor: Roberto Daniel Ramírez Díaz de León
; 07/11/2023 02:19:42 p. m.
include 'emu8086.inc'
org 100h
MOV AX, 258
PUSH AX
POP AX
; Asignacion a
MOV a, AX
MOV AX, a
PUSH AX
MOV BX, 256
DIV BX
PUSH DX
XOR DX, DX
POP AX
; Asignacion a
MOV a, AX
MOV AX, 8
PUSH AX
POP AX
MOV BX, a
ADD AX, BX
MOV a, AX
MOV AX, 10
PUSH AX
POP AX
MOV BX, a
IMUL BX
MOV a, AX
MOV AX, 100
PUSH AX
POP AX
MOV BX, AX
MOV AX, a
IDIV BX
MOV a, AX
PRINT 'Valor Casteado de a: '
MOV AX,a
CALL PRINT_NUM
PRINTN ''
PRINT ''
PRINTN ' '
PRINT 'Digite el valor de altura: '
CALL SCAN_NUM
MOV altura, CX
PRINTN ' '
PRINT ''
PRINTN ' '
PRINT 'for:'
PRINTN ' '
PRINT ''
; FOR: 1
MOV AX, 1
PUSH AX
POP AX
; Asignacion i
MOV i, AX
InicioFor1:
MOV AX, i
PUSH AX
MOV AX, altura
PUSH AX
POP BX
POP AX
CMP AX, BX
JA FinFor1
PRINT '  '
; FOR: 2
MOV AX, 250
PUSH AX
POP AX
; Asignacion j
MOV j, AX
InicioFor2:
MOV AX, j
PUSH AX
MOV AX, 250
PUSH AX
MOV AX, i
PUSH AX
POP BX
POP AX
ADD AX, BX
PUSH AX
POP BX
POP AX
CMP AX, BX
JAE FinFor2
; if: 1
MOV AX, j
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
DIV  BX
PUSH DX
MOV AX, 0
PUSH AX
POP BX
POP AX
CMP AX, BX
JNE Eif1
PRINT '-'
; else: 1
JMP Eelse1
Eif1:
PRINT '+'
Eelse1:
INC j
JMP InicioFor2
FinFor2:
PRINT ''
PRINTN ' '
PRINT ''
INC i
JMP InicioFor1
FinFor1:
PRINT ''
PRINTN ' '
PRINT 'while:'
PRINTN ' '
PRINT ''
MOV AX, 1
PUSH AX
POP AX
; Asignacion i
MOV i, AX
; While: 1
InicioWhile1:
MOV AX, i
PUSH AX
MOV AX, altura
PUSH AX
POP BX
POP AX
CMP AX, BX
JA FinWhile1
PRINT '  '
MOV AX, 250
PUSH AX
POP AX
; Asignacion j
MOV j, AX
; While: 2
InicioWhile2:
MOV AX, j
PUSH AX
MOV AX, 250
PUSH AX
MOV AX, i
PUSH AX
POP BX
POP AX
ADD AX, BX
PUSH AX
POP BX
POP AX
CMP AX, BX
JAE FinWhile2
; if: 2
MOV AX, j
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
DIV  BX
PUSH DX
MOV AX, 0
PUSH AX
POP BX
POP AX
CMP AX, BX
JNE Eif2
PRINT '-'
; else: 2
JMP Eelse2
Eif2:
PRINT '+'
Eelse2:
MOV AX, j
INC AX
MOV j, AX
JMP InicioWhile2
FinWhile2:
MOV AX, i
INC AX
MOV i, AX
PRINT ''
PRINTN ' '
PRINT ''
JMP InicioWhile1
FinWhile1:
PRINT ''
PRINTN ' '
PRINT 'do:'
PRINTN ' '
PRINT ''
MOV AX, 1
PUSH AX
POP AX
; Asignacion i
MOV i, AX
; Do: 1
InicioDo1:
PRINT '  '
MOV AX, 250
PUSH AX
POP AX
; Asignacion j
MOV j, AX
; Do: 2
InicioDo2:
; if: 3
MOV AX, j
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
DIV  BX
PUSH DX
MOV AX, 0
PUSH AX
POP BX
POP AX
CMP AX, BX
JNE Eif3
PRINT '-'
; else: 3
JMP Eelse3
Eif3:
PRINT '+'
Eelse3:
MOV AX, j
INC AX
MOV j, AX
MOV AX, j
PUSH AX
MOV AX, 250
PUSH AX
MOV AX, i
PUSH AX
POP BX
POP AX
ADD AX, BX
PUSH AX
POP BX
POP AX
CMP AX, BX
JAE FinDo2
JMP InicioDo2
FinDo2:
MOV AX, i
INC AX
MOV i, AX
PRINT ''
PRINTN ' '
PRINT ''
MOV AX, i
PUSH AX
MOV AX, altura
PUSH AX
POP BX
POP AX
CMP AX, BX
JA FinDo1
JMP InicioDo1
FinDo1:
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
a dw 0h 
b dw 0h 
END
