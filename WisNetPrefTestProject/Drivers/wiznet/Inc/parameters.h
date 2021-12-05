/*
 * parameters.h
 *
 *  Created on: 24 нояб. 2021 г.
 *      Author: user
 */

#ifndef WIZNET_INC_PARAMETERS_H_
#define WIZNET_INC_PARAMETERS_H_

/**
 * переменные для настройки вывода
 */

//#define TX_END_CR
//#define TX_END_LF
#define TX_END_CRLF

/*
 * Максимальная длина для имени параметра
 */
#define NAME_LENGTH 16

typedef enum {
	TYPE_BOOL, // эмулируем бинарную переменную - для выключателей
	TYPE_CHAR, // 1 байт знаковый
	TYPE_UCHAR, // 1 байт беззнаковый
	TYPE_INT,  // 2 байта знаковый
	TYPE_UINT,  // 2 байта беззнаковый
	TYPE_LONG, // 4 байта знаковый
	TYPE_ULONG, // 4 байта беззнаковый
	TYPE_FLOAT, // плавающая точка одинарной точности
	TYPE_DOUBLE, // плавающая точка двойной точности
	TYPE_ASCIICHAR, // одиночный символ
	TYPE_STRING, // строка
	TYPE_MACROS // команда - имеем команду = вызываем функцию.
} parameter_formats;
typedef enum {
	COMMAND_ECHO_OK, // Получили OK в любом регистре
	COMMAND_LIST_OF_VARIABLES, // получили LIST - отвечаем списком доступных для редактирования переменных (если границы не определены - выводим дефолтные для типа)
	COMMAND_LIST_OF_COMMANDS, // получили RUNNABLE - отвечаем списком команд с типом TYPE_MACROS
	COMMAND_GET_VARIABLE, // получили GET @VAR - отвечаем текстовым эквивалентом значения этой переменной в зависимости от её типа
	COMMAND_SET_VARIABLE, // Получили SET VAR=значение - парсим и выполняем GET
	COMMAND_RUN, // получили RUN - и вызываем действие по указателю, главное чтобы у вызываемого метода не было входныъ или выходных параметров.
} command_types;
typedef struct parameter_record_t {
	char name[NAME_LENGTH]; /*Название переменной*/
	char format; //формат переменной из перечисления parameter_formats
	void* dimensions; //указатель на адрес, где хранится структура с размерами либо ссылка на параметр в макросе
	void* value; //указатель на значение переменной
} parameter_record;

typedef struct char_dimensions_t {
	char minVal;// = -128;
	char maxVal;// = 127;
} char_dimensions;

typedef struct uchar_dimensions_t {
	unsigned char minVal;// = 0;
	unsigned char maxVal;// = 255;
} uchar_dimensions;

typedef struct int_dimensions_t {
	char minVal;// = -32768;
	char maxVal;// = 32767;
} int_dimensions;

typedef struct uint_dimensions_t {
	unsigned char minVal;// = 0;
	unsigned char maxVal;// = 65535;
} uint_dimensions;

typedef struct long_dimensions_t {
	long minVal;// = -2147483648;
	long maxVal;// = 2147483647;
} long_dimensions;

typedef struct ulong_dimensions_t {
	unsigned long minVal;// = 0;
	unsigned long maxVal;// = 4294967295;
} ulong_dimensions;

typedef struct float_dimensions_t {
	float minVal;// = -100.00;
	float maxVal;// = 100.00;
	float defaultStep;// = 0.01;
} float_dimensions;

typedef struct double_dimensions_t {
	float minVal;// = -100.0000;
	float maxVal;// = 100.0;
	float defaultStep;// = 0.0001;
} double_dimensions;

typedef struct string_dimensions_t{
	char charsCount;
} string_dimensions;


void linkParametersStorage(parameter_record parameter_records_array[], uint8_t parameters_count);

#endif /* WIZNET_INC_PARAMETERS_H_ */
