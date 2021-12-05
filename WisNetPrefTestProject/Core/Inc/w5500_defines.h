/*
 * w5500_defines.h
 *
 *  Created on: Nov 14, 2021
 *      Author: user
 */

#ifndef INC_W5500_DEFINES_H_
#define INC_W5500_DEFINES_H_

#include "socket.h"
#include "dhcp.h"
#include "dns.h"
#include "telnetConfigServer.h"

#include "wizchip_conf.h"
#endif /* INC_W5500_DEFINES_H_ */

/* USER CODE BEGIN PD */
extern SPI_HandleTypeDef hspi1;

#define DHCP_SOCKET     0
#define DNS_SOCKET      1
#define HTTP_SOCKET     2

/* USER CODE END PD */
wiz_NetInfo net_info = {
	              .mac  = { 0xAA, 0x23, 0x45, 0x43, 0xD3, 0xFF },
				  .ip = {192,168,1,252},
				  .sn = {255,255,255,0},
				  .gw = {192,168,1,1},
	              .dns = {8,8,8,8}, //по дефолту тулим гугловский DNS
	              .dhcp = NETINFO_DHCP
	          };
// 1K should be enough, see https://forum.wiznet.io/t/topic/1612/2
uint8_t dhcp_buffer[1024];
// 1K seems to be enough for this buffer as well
uint8_t dns_buffer[1024];

//uint8_t dns[4];
void W5500_Select(void) {
    HAL_GPIO_WritePin(W5500_CS_GPIO_Port, W5500_CS_Pin, GPIO_PIN_RESET);
}

void W5500_Unselect(void) {
    HAL_GPIO_WritePin(W5500_CS_GPIO_Port, W5500_CS_Pin, GPIO_PIN_SET);
}

void W5500_ReadBuff(uint8_t* buff, uint16_t len) {
    HAL_SPI_Receive(&hspi1, buff, len, HAL_MAX_DELAY);
}

void W5500_WriteBuff(uint8_t* buff, uint16_t len) {
    HAL_SPI_Transmit(&hspi1, buff, len, HAL_MAX_DELAY);
}

uint8_t W5500_ReadByte(void) {
    uint8_t byte;
    W5500_ReadBuff(&byte, sizeof(byte));
    return byte;
}

void W5500_WriteByte(uint8_t byte) {
    W5500_WriteBuff(&byte, sizeof(byte));
}
