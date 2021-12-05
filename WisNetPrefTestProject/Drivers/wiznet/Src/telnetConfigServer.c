/*
 * telnetConfigServer.c
 *
 *  Created on: 23 нояб. 2021 г.
 *      Author: user
 */
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <stdint.h>
#include "socket.h"
#include "parameters.h"
#include "telnetConfigServer.h"

uint16_t config_port = 8080;

uint8_t lastSocketStatus;
uint8_t lastSocketProcess;
uint8_t socket_is_active;
// Реакция на прерывание по ноге


int32_t telnetConfigServer_InterruptCallback() {
//	 loopback_tcps(uint8_t sn, uint8_t* buf, uint16_t port)
 if (socket_is_active){
	   int32_t ret;
	   uint16_t size = 0, sentsize=0;
	   lastSocketStatus = getSn_SR(TELNET_SOCKET);
	   switch(lastSocketStatus)
	   {
	      case SOCK_ESTABLISHED:
	         if(getSn_IR(TELNET_SOCKET) & Sn_IR_CON)
	         {lastSocketProcess = SOCKET_RECEIVE;

     			setSn_IR(TELNET_SOCKET,Sn_IR_CON);
	         }
			 if((size = getSn_RX_RSR(TELNET_SOCKET)) > 0) // Don't need to check SOCKERR_BUSY because it doesn't not occur.
	         {
				if(size > DATA_BUF_SIZE) size = DATA_BUF_SIZE;
				ret = recv(TELNET_SOCKET, tcp_buffer, size);
				lastSocketProcess = SOCKET_PROCESSING;
				ret = parseCommand(tcp_buffer,ret, tcp_buffer);
				if(ret <= 0) return ret;      // check SOCKERR_BUSY & SOCKERR_XXX. For showing the occurrence of SOCKERR_BUSY.
				size = (uint16_t) ret;
				sentsize = 0;


				lastSocketProcess = SOCKET_TRANSMIT;
				while(size != sentsize)
				{
					ret = send(TELNET_SOCKET, tcp_buffer+sentsize, size-sentsize);
					if(ret < 0)
					{
						close(TELNET_SOCKET);
						return ret;
					}
					sentsize += ret; // Don't care SOCKERR_BUSY, because it is zero.
				}
	         } else {
	        	 lastSocketProcess = SOCKET_IDLE;
	         }
	         break;
	      case SOCK_CLOSE_WAIT :
	    	   lastSocketProcess = SOCKET_ERROR;
	    	  if((ret = disconnect(TELNET_SOCKET)) != SOCK_OK) return ret;
	         break;
	      case SOCK_INIT :
	    	   lastSocketProcess = SOCKET_ERROR;

	         if( (ret = listen(TELNET_SOCKET)) != SOCK_OK) return ret;
	         break;
	      case SOCK_CLOSED:
	    	   lastSocketProcess = SOCKET_ERROR;
	         if((ret = socket(TELNET_SOCKET, Sn_MR_TCP, config_port, SF_TCP_NODELAY)) != TELNET_SOCKET) return ret;
	         break;
	      default:
	         break;
	   }
 }
	   return 1;
};

void telnetConfigServer_Init(uint16_t port) {
config_port = port;
lastSocketStatus = SOCK_INIT;
lastSocketProcess = SOCKET_ERROR;						close(TELNET_SOCKET);

socket(TELNET_SOCKET, Sn_MR_TCP, config_port, SF_TCP_NODELAY);
};
uint16_t telnetConfigServer_getPort(){
	return config_port;
}
void telnetConfigServer_registerParameters(/*ParameterList *params*/){

}

int32_t telnetConfigServer_SocketStatus() {
	return lastSocketStatus;
}
uint8_t telnetConfigServer_SocketProcess() {
	return lastSocketProcess;
}
void telnetConfigServer_SocketOpen() {
	socket_is_active = 1;
}

void telnetConfigServer_SocketClose() {
	lastSocketStatus = SOCK_CLOSED;
	lastSocketProcess = SOCKET_ERROR;
	close(TELNET_SOCKET);

	disconnect(TELNET_SOCKET);
	socket_is_active = 0;
}
