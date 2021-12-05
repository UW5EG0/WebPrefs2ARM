/*
 * parameters.c
 *
 *  Created on: 24 нояб. 2021 г.
 *      Author: user
 */
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <stdint.h>
#include <parameters.h>

uint8_t p_count;
parameter_record* p_pointer;
char _current_command = -1;
u_long _chars_to_send = 0;
unsigned int _var_name_length = 0;
char _command_text[NAME_LENGTH+8] = ""; //в 8 знаков у нас влезет любое разрешенное имя команды

/**
 * Передаём указатель на массив со структурой
 */
void linkParametersStorage(parameter_record parameter_records_array[], uint8_t parameters_count)
{
	p_count = parameters_count;
	p_pointer = parameter_records_array;
}
/*
 *
 */
uint8_t processParameter(uint32_t buf_pointer , parameter_record *parameter){
	return 0;
}

int8_t getIndexOfVariable(char *variable_text) {
	for (int var_idx = 0; var_idx<p_count;var_idx++)
			{
			if (strncmp(p_pointer[var_idx].name,variable_text,_var_name_length) == 0)
			 return var_idx;
			}
	return -1; //не найдено

}

void clearCommandBuffer()
{
	for(_var_name_length = 0; _var_name_length <NAME_LENGTH+8; _var_name_length++){
		_command_text[_var_name_length] = 0;
	}
}

void showVariable(char *_variable, char *tx_buffer)
{

}

int16_t seekToChar(char *buffer, uint16_t length, char someChar)
{
for (uint16_t charIdx = 0; charIdx < length; charIdx++){
	if (buffer[charIdx] == someChar) return charIdx;
}
return -1;
}
uint8_t varToString(parameter_record* variable, char *buffer)
{
	switch (variable->format){
			case TYPE_BOOL:
			case TYPE_CHAR:		 return sprintf(buffer,"%i",*(signed char *)variable->value); break;
			case TYPE_UCHAR: 	 return sprintf(buffer,"%d",*(uint8_t *)variable->value); break;
			case TYPE_INT: 		 return sprintf(buffer,"%i",*(signed int *)variable->value); break;
			case TYPE_UINT: 	 return sprintf(buffer,"%i",*(unsigned int *)variable->value); break;
			case TYPE_LONG: 	 return sprintf(buffer,"%l",*(signed long *)variable->value); break;
			case TYPE_ULONG: 	 return sprintf(buffer,"%l",*(unsigned long *)variable->value); break;
		    case TYPE_FLOAT: 	 return sprintf(buffer,"%f",*(float *)variable->value); break;
			case TYPE_DOUBLE: 	 return sprintf(buffer,"%f",*(double *)variable->value); break;
			case TYPE_ASCIICHAR: return sprintf(buffer,"%c",*(char *)variable->value); break;
			case TYPE_STRING: 	 return sprintf(buffer,"%s",(char *)(variable->value)); break;
			default: return 0;
	}
}

uint8_t varRangeToString(parameter_record* variable, char *buffer)
{
	uint8_t answer = 0;
	switch (variable->format){
			case TYPE_BOOL:		 return sprintf(buffer,"0:1:1"); break;
			case TYPE_CHAR:
				answer +=sprintf(buffer+answer,"%i",(variable->dimensions > 0)?(*(char_dimensions *)variable->dimensions).minVal:-128);
				answer +=sprintf(buffer+answer,":%i",(variable->dimensions > 0)?(*(char_dimensions *)variable->dimensions).maxVal:127);
				break;
			case TYPE_ASCIICHAR:
			case TYPE_UCHAR:
				answer +=sprintf(buffer+answer,"%u",(variable->dimensions > 0)?(*(uchar_dimensions *)variable->dimensions).minVal:0);
				answer +=sprintf(buffer+answer,":%u",(variable->dimensions > 0)?(*(uchar_dimensions *)variable->dimensions).maxVal:255);
				 break;
			case TYPE_INT:
				answer +=sprintf(buffer+answer,"%i",(variable->dimensions > 0)?(*(int_dimensions *)variable->dimensions).minVal:-32768);
				answer +=sprintf(buffer+answer,":%i",(variable->dimensions > 0)?(*(int_dimensions *)variable->dimensions).maxVal:32767);
				break;
			case TYPE_UINT:
				answer +=sprintf(buffer+answer,"%u",(variable->dimensions > 0)?(*(uint_dimensions *)variable->dimensions).minVal:0);
				answer +=sprintf(buffer+answer,":%u",(variable->dimensions > 0)?(*(uint_dimensions *)variable->dimensions).maxVal:65535);
				break;
			case TYPE_LONG:
				answer +=sprintf(buffer+answer,"%l",((variable->dimensions > 0)?(*(long_dimensions *)variable->dimensions).minVal:-2147483647));
				answer +=sprintf(buffer+answer,":%l",((variable->dimensions > 0)?(*(long_dimensions *)variable->dimensions).maxVal:-2147483647));
				break;
			case TYPE_ULONG:
				answer +=sprintf(buffer+answer,"%lu",((variable->dimensions > 0)?(*(ulong_dimensions *)variable->dimensions).minVal:0));
				answer +=sprintf(buffer+answer,":%ll",((variable->dimensions > 0)?(*(ulong_dimensions *)variable->dimensions).maxVal:4294967295));
			break;
		    case TYPE_FLOAT:
		    	answer +=sprintf(buffer+answer,"%f",(variable->dimensions > 0)?(*(float_dimensions *)variable->dimensions).minVal:-100);
		    	answer +=sprintf(buffer+answer,":%f",(variable->dimensions > 0)?(*(float_dimensions *)variable->dimensions).maxVal:100);
		    	answer +=sprintf(buffer+answer,":%f",(variable->dimensions > 0)?(*(float_dimensions *)variable->dimensions).defaultStep:0.01);
		    	break;
			case TYPE_DOUBLE:
				answer +=sprintf(buffer+answer,"%f",(variable->dimensions > 0)?(*(float_dimensions *)variable->dimensions).minVal:-100);
				answer +=sprintf(buffer+answer,":%f",(variable->dimensions > 0)?(*(float_dimensions *)variable->dimensions).maxVal:100);
				answer +=sprintf(buffer+answer,":%f",(variable->dimensions > 0)?(*(float_dimensions *)variable->dimensions).defaultStep:0.01);
			break;
			case TYPE_STRING:
				answer += sprintf(buffer+answer,"%i",(variable->dimensions > 0)?(*(string_dimensions *)variable->dimensions).charsCount:32);
				break;
			default: return 0;
	}
	return answer;
}
/**
 * Расшифровываем, чаво хотели от микрухи
 */
uint32_t parseCommand(char *rx_buffer, uint32_t rx_seek, char *tx_buffer)
{
	_chars_to_send = 0;
	unsigned int idxOfCharInBuffer = 0;
	unsigned int idxOfParameter = 0;

	int8_t var_idx = -1;
	while (rx_buffer[idxOfCharInBuffer]>0 && rx_buffer[idxOfCharInBuffer]!=' '){
		_command_text[idxOfCharInBuffer] = rx_buffer[idxOfCharInBuffer];
		idxOfCharInBuffer++;
	}
	_current_command = -1;
	if (strncmp(strlwr(_command_text), "ok",2)== 0){
			_current_command = COMMAND_ECHO_OK;
		}
	if (strncmp(strlwr(_command_text), "list",4)== 0){
				_current_command = COMMAND_LIST_OF_VARIABLES;
			}
	if (strncmp(strlwr(_command_text), "get",3)== 0){
						_current_command = COMMAND_GET_VARIABLE;
					}
	if (strncmp(strlwr(_command_text), "set",3)== 0){
							_current_command = COMMAND_SET_VARIABLE;
						}
	if (strncmp(strlwr(_command_text), "run",3)== 0){
							_current_command = COMMAND_RUN;
						}
	if (strncmp(strlwr(_command_text), "runnable",8)== 0){
						_current_command = COMMAND_LIST_OF_COMMANDS;
					}

	switch(_current_command)
	{
	case COMMAND_ECHO_OK: break; // Получили OK - ответ пустой
	/*
	 *  получили LIST - отвечаем списком доступных для редактирования переменных
	 *  (если границы не определены - выводим дефолтные для типа)
	 */
	case COMMAND_LIST_OF_VARIABLES:

		for (idxOfParameter = 0; idxOfParameter<p_count;idxOfParameter++)
		{
		if (p_pointer[idxOfParameter].format < TYPE_MACROS) {
			_chars_to_send += sprintf(tx_buffer+_chars_to_send,"%s ",p_pointer[idxOfParameter].name); //имя
				switch (p_pointer[idxOfParameter].format){
					case TYPE_BOOL:
					case TYPE_CHAR:
					case TYPE_UCHAR:
					case TYPE_INT:
					case TYPE_UINT:
					case TYPE_LONG:
					case TYPE_ULONG: _chars_to_send += sprintf(tx_buffer+_chars_to_send,"int ["); break;
					case TYPE_FLOAT:
					case TYPE_DOUBLE: _chars_to_send += sprintf(tx_buffer+_chars_to_send,"flt ["); break;
					case TYPE_ASCIICHAR:
					case TYPE_STRING: _chars_to_send += sprintf(tx_buffer+_chars_to_send,"str ["); break;
					}
				_chars_to_send += varRangeToString(&p_pointer[idxOfParameter], tx_buffer+_chars_to_send);
				_chars_to_send += sprintf(tx_buffer+_chars_to_send,"]=");
				_chars_to_send += varToString(&p_pointer[idxOfParameter], tx_buffer+_chars_to_send);
#if(defined TX_END_CRLF || defined TX_END_CR)
				tx_buffer[_chars_to_send++]='\r';
#endif
#if(defined TX_END_CRLF || defined TX_END_LF)
				tx_buffer[_chars_to_send++]='\n';
#endif
		}
	}

		break;
		/*
		 *  получили RUNNABLE - отвечаем списком доступных для вызова макросов
		 */
	case COMMAND_LIST_OF_COMMANDS:
		for (idxOfParameter = 0; idxOfParameter<p_count;idxOfParameter++)
				{
				if (p_pointer[idxOfParameter].format == TYPE_MACROS)
				_chars_to_send += sprintf(tx_buffer+_chars_to_send,"%s ",p_pointer[idxOfParameter].name); //имя
#if(defined TX_END_CRLF || defined TX_END_CR)
				tx_buffer[_chars_to_send++]='\r';
#endif
#if(defined TX_END_CRLF || defined TX_END_LF)
				tx_buffer[_chars_to_send++]='\n';
#endif
				}
		break;
		/*
		 *  получили RUNNABLE - отвечаем списком доступных для вызова макросов
		 */

	case COMMAND_GET_VARIABLE:
		clearCommandBuffer();
		_var_name_length = 0;
		while (rx_buffer[idxOfCharInBuffer]>0 && rx_buffer[idxOfCharInBuffer]!='\r' && rx_buffer[idxOfCharInBuffer]!='\n'){
					if (rx_buffer[idxOfCharInBuffer] !=' '){
						_command_text[_var_name_length] = rx_buffer[idxOfCharInBuffer]; //записываем
						_var_name_length++;
					}
					idxOfCharInBuffer++;
				}

		var_idx = getIndexOfVariable(_command_text);

		if (var_idx == -1) {
			_chars_to_send += sprintf(tx_buffer+_chars_to_send,"UNKNOWN_VARIABLE\n");
		}
		else
		{
			_chars_to_send += varToString(&p_pointer[var_idx], tx_buffer);
#if(defined TX_END_CRLF || defined TX_END_CR)
				tx_buffer[_chars_to_send++]='\r';
#endif
#if(defined TX_END_CRLF || defined TX_END_LF)
				tx_buffer[_chars_to_send++]='\n';
#endif
		}

	break; // получили GET @VAR - отвечаем текстовым эквивалентом значения этой переменной в зависимости от её типа
	case COMMAND_SET_VARIABLE:
		clearCommandBuffer();
			_var_name_length = 0;
			while (rx_buffer[idxOfCharInBuffer]>0 && rx_buffer[idxOfCharInBuffer]!='=' && rx_buffer[idxOfCharInBuffer]!='\r' && rx_buffer[idxOfCharInBuffer]!='\n'){
						if (rx_buffer[idxOfCharInBuffer] !=' '){
							_command_text[_var_name_length] = rx_buffer[idxOfCharInBuffer]; //записываем
							_var_name_length++;
						}
						idxOfCharInBuffer++;
					}
			var_idx = getIndexOfVariable(_command_text);
			int16_t seek = seekToChar(rx_buffer, 1024,'=');
			if (seek == -1)
				_chars_to_send = sprintf(tx_buffer, "VALUE_NOT_FOUND");
			else
			switch (p_pointer[var_idx].format){
					case TYPE_BOOL:
						(*(char *)p_pointer[var_idx].value) = (rx_buffer[seek] == '1')?(1):(0);
					break;
					case TYPE_CHAR:
						(*(char *)p_pointer[var_idx].value) = atoi(rx_buffer+seek+1);
					break;
					case TYPE_UCHAR:
						(*(unsigned char *)p_pointer[var_idx].value) = atoi(rx_buffer+seek+1);
					break;
					case TYPE_INT:
						(*(int *)p_pointer[var_idx].value) = atoi(rx_buffer+seek+1);
					break;
					case TYPE_UINT:
						(*(unsigned int *)p_pointer[var_idx].value) = atoi(rx_buffer+seek+1);
					break;
					case TYPE_LONG:
						(*(long *)p_pointer[var_idx].value) = atol(rx_buffer+seek+1);
					break;
					case TYPE_ULONG:
						(*(unsigned long *)p_pointer[var_idx].value) = atol(rx_buffer+seek+1);
					break;
					case TYPE_FLOAT:
						(*(float *)p_pointer[var_idx].value) = atoff(rx_buffer+seek+1);
					break;
					case TYPE_DOUBLE:
						(*(double *)p_pointer[var_idx].value) = atof(rx_buffer+seek+1);
					break;
					case TYPE_ASCIICHAR:
						(*(char *)p_pointer[var_idx].value) = rx_buffer[seek+1];
					break;
					case TYPE_STRING:
						_chars_to_send = 0;
						seek = seekToChar(rx_buffer, 1024,'=');
						uint8_t newStringLength = 0;
						//Очищаем переменную
						for (idxOfCharInBuffer = 0; idxOfCharInBuffer < (*(string_dimensions *)p_pointer[var_idx].dimensions).charsCount; idxOfCharInBuffer++)
						{(*(char *)(p_pointer[var_idx].value+idxOfCharInBuffer)) = 0;}

						while (rx_buffer[seek+_chars_to_send]!='\r' &&
							   rx_buffer[seek+_chars_to_send]!='\n' &&
							   (*(string_dimensions *)p_pointer[var_idx].dimensions).charsCount >= _chars_to_send)
						{
							if (rx_buffer[seek+_chars_to_send+1]!='\r' &&
							   rx_buffer[seek+_chars_to_send+1]!='\n')
							(*(char *)(p_pointer[var_idx].value+_chars_to_send))=rx_buffer[_chars_to_send+seek+1];
							_chars_to_send++;
						}
						//записываем вывод
						for (idxOfCharInBuffer = 0; idxOfCharInBuffer < (*(string_dimensions *)p_pointer[var_idx].dimensions).charsCount; idxOfCharInBuffer++)
						{
							tx_buffer[idxOfCharInBuffer] = (*(char *)(p_pointer[var_idx].value+idxOfCharInBuffer));
						}

						tx_buffer[(*(string_dimensions *)p_pointer[var_idx].dimensions).charsCount]= 0;
						_chars_to_send = 0;
					break;
				}

			_chars_to_send += varToString(&p_pointer[var_idx], tx_buffer+_chars_to_send);
#if(defined TX_END_CRLF || defined TX_END_CR)
				tx_buffer[_chars_to_send++]='\r';
#endif
#if(defined TX_END_CRLF || defined TX_END_LF)
				tx_buffer[_chars_to_send++]='\n';
#endif
		break; // Получили SET VAR=значение - парсим и выполняем GET

		case COMMAND_RUN:
			clearCommandBuffer();
			_var_name_length = 0;
			while (rx_buffer[idxOfCharInBuffer]>0 && rx_buffer[idxOfCharInBuffer]!='\r' && rx_buffer[idxOfCharInBuffer]!='\n'){
						if (rx_buffer[idxOfCharInBuffer] !=' '){
							_command_text[_var_name_length] = rx_buffer[idxOfCharInBuffer]; //записываем
							_var_name_length++;
						}
						idxOfCharInBuffer++;
					}

			var_idx = getIndexOfVariable(_command_text);

			if (var_idx == -1) {
				_chars_to_send += sprintf(tx_buffer+_chars_to_send,"UNKNOWN_VARIABLE\n");
			}
			else
			{
				if (p_pointer[var_idx].format == TYPE_MACROS) {
					_chars_to_send = sprintf(tx_buffer,"EXECUTE %s ...\r\n", p_pointer[var_idx].name);
					void (*callback) (void);
					callback = p_pointer[var_idx].value;
					callback(); //вызываем как колбек
				} else {
					_chars_to_send += sprintf(tx_buffer+_chars_to_send,"%s IS NOT MACROS\r\n", p_pointer[var_idx].name);
				}
			}
			break;
	default: _chars_to_send = rx_seek + 1 + sprintf(tx_buffer + 1 + rx_seek ," = UNKNOWN_COMMAND\r\n");
	}
	tx_buffer[_chars_to_send++]='O';
	tx_buffer[_chars_to_send++]='K';
#if(defined TX_END_CRLF || defined TX_END_CR)
				tx_buffer[_chars_to_send++]='\r';
#endif
#if(defined TX_END_CRLF || defined TX_END_LF)
				tx_buffer[_chars_to_send++]='\n';
#endif
	tx_buffer[_chars_to_send++]=0;
		return _chars_to_send;
}
