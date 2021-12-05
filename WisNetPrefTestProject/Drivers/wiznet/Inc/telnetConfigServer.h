/*
 * telnetConfigServer.h
 *
 *  Created on: 23 нояб. 2021 г.
 *      Author: user
 */

#ifndef WIZNET_INC_TELNETCONFIGSERVER_H_
#define WIZNET_INC_TELNETCONFIGSERVER_H_

typedef enum {SOCKET_ERROR,
	SOCKET_IDLE,
	SOCKET_RECEIVE,
	SOCKET_PROCESSING,
	SOCKET_TRANSMIT,

}
socket_process;

//* Defines
#define TELNET_SOCKET   3
// 1k for encoding and transmitting commands
uint8_t tcp_buffer[1024];

/* DATA_BUF_SIZE define for Loopback example */
#ifndef DATA_BUF_SIZE
	#define DATA_BUF_SIZE			2048
#endif

/************************/
/* Select LOOPBACK_MODE */
/************************/
#define LOOPBACK_MAIN_NOBLOCK    0
#define LOOPBACK_MODE   LOOPBACK_MAIN_NOBLOCK

//
void telnetConfigServer_Init(uint16_t port);
uint16_t telnetConfigServer_getPort();
int32_t telnetConfigServer_InterruptCallback();
int32_t telnetConfigServer_SocketStatus();
uint8_t telnetConfigServer_SocketProcess();
void telnetConfigServer_SocketClose();
void telnetConfigServer_SocketOpen();
#endif /* WIZNET_INC_TELNETCONFIGSERVER_H_ */
