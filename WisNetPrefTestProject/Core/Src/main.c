/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.c
  * @brief          : Main program body
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; Copyright (c) 2021 STMicroelectronics.
  * All rights reserved.</center></h2>
  *
  * This software component is licensed by ST under BSD 3-Clause license,
  * the "License"; You may not use this file except in compliance with the
  * License. You may obtain a copy of the License at:
  *                        opensource.org/licenses/BSD-3-Clause
  *
  ******************************************************************************
  */
/* USER CODE END Header */
/* Includes ------------------------------------------------------------------*/
#include "main.h"

/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */
#include "stdio.h"
#include "image.h"
#include "ssd1306.h"
#include "w5500_defines.h"
#include "parameters.h"
/* USER CODE END Includes */

/* Private typedef -----------------------------------------------------------*/
/* USER CODE BEGIN PTD */
typedef enum {
	MENU_IDLE = 0,
	MENU_PHY_STATS,
	MENU_IP_STATS,
	MENU_DNS,
	MENU_TELNET,

} menus ;

/* USER CODE END PTD */

/* Private define ------------------------------------------------------------*/
/* USER CODE BEGIN PD */

/**
 * Количество параметров в массиве свойств
*/
#define PARAMS_COUNT 6
/* USER CODE END PD */

/* Private macro -------------------------------------------------------------*/
/* USER CODE BEGIN PM */

/* USER CODE END PM */

/* Private variables ---------------------------------------------------------*/
I2C_HandleTypeDef hi2c1;
DMA_HandleTypeDef hdma_i2c1_tx;

SPI_HandleTypeDef hspi1;

TIM_HandleTypeDef htim1;

/* USER CODE BEGIN PV */

/**
 * инициируем перечень параметров
 */
parameter_record params[PARAMS_COUNT];

uint16_t menu_idx;
char line[20];
char status[20];
char TelnetStatus[20];
char retries = 0;
wiz_PhyConf PhyStatus;
uint8_t linkStatus,oldlinkStatus;
/* USER CODE END PV */

/* Private function prototypes -----------------------------------------------*/
void SystemClock_Config(void);
static void MX_GPIO_Init(void);
static void MX_DMA_Init(void);
static void MX_I2C1_Init(void);
static void MX_SPI1_Init(void);
static void MX_TIM1_Init(void);
/* USER CODE BEGIN PFP */

/* USER CODE END PFP */

/* Private user code ---------------------------------------------------------*/
/* USER CODE BEGIN 0 */
char var_a = -26;
float var_b = 25.234;
char var_c[20] = "test_a";
unsigned char var_d = 26;
double var_e = 67.1235;
char var_f[30] = "Test b";
string_dimensions st_var_c = {.charsCount = 20};
string_dimensions st_var_f = {.charsCount = 30};

void initParameters()
{
	for(uint8_t i = 0; i< PARAMS_COUNT; i++)
		sprintf(params[i].name, "VAR_%c",('A'+i));
			 params[0].format = TYPE_CHAR;
			 params[1].format = TYPE_FLOAT;
			 params[2].format = TYPE_STRING;
			 params[3].format = TYPE_UCHAR;
			 params[4].format = TYPE_DOUBLE;
			 params[5].format = TYPE_STRING;

	params[0].value = &var_a;
	params[1].value = &var_b;
	params[2].value = &var_c;
	params[2].dimensions = &st_var_c;
	params[3].value = &var_d;
	params[4].value = &var_e;
	params[5].value = &var_f;
	params[5].dimensions = &st_var_f;

}

volatile char ip_assigned = 0;

void Callback_IPAssigned(void) {
    //UART_Printf("Callback: IP assigned! Leased time: %d sec\r\n", getDHCPLeasetime());
    ip_assigned = 1;
}

void Callback_IPConflict(void) {
    //UART_Printf("Callback: IP conflict!\r\n");
}

void draw_menu(int menu_index) {
	 ssd1306_Clear();
     ssd1306_SetColor(White);
if (menu_index > 0){
     ssd1306_SetCursor(0, 0);
	    sprintf(line,"%d", menu_index);
	    ssd1306_WriteString(line, Font_7x10);
}
	switch (menu_index){
	case MENU_PHY_STATS: //PHY stats
		ssd1306_SetCursor(8, 0);
		ssd1306_WriteString("PHY stats", Font_7x10);
		ssd1306_SetCursor(80, 0);
		if (PhyStatus.by == PHY_CONFBY_HW)
				ssd1306_WriteString("by HW", Font_7x10);
		if (PhyStatus.by == PHY_CONFBY_SW)
				ssd1306_WriteString("by SW", Font_7x10);

			    ssd1306_SetCursor(0, 11);
			    sprintf(line,"%02X:%02X:%02X:%02X:%02X:%02X",net_info.mac[0],
			    											 net_info.mac[1],
															 net_info.mac[2],
															 net_info.mac[3],
															 net_info.mac[4],
															 net_info.mac[5]);
			    ssd1306_WriteString(line, Font_7x10);
			    ssd1306_SetCursor(0, 22);
			    if (PhyStatus.speed == PHY_SPEED_10)
			    ssd1306_WriteString("10mBit", Font_7x8);
			    if (PhyStatus.speed == PHY_SPEED_100)
			    ssd1306_WriteString("100mBit", Font_7x8);
			    ssd1306_SetCursor(0070, 22);
			    if (PhyStatus.duplex == PHY_DUPLEX_HALF)
			    ssd1306_WriteString("Half", Font_7x8);
			   	if (PhyStatus.duplex == PHY_DUPLEX_FULL)
			   	ssd1306_WriteString("Full", Font_7x8);
			   	ssd1306_SetCursor(90, 22);
			   						    if (PhyStatus.mode == PHY_MODE_MANUAL)
			   						    ssd1306_WriteString("man", Font_7x8);
			   						   if (PhyStatus.mode == PHY_MODE_AUTONEGO)
			   						   ssd1306_WriteString("auto", Font_7x8);


	break;
	case MENU_IP_STATS: //IPv4
		ssd1306_SetCursor(8, 0);
        ssd1306_WriteString("IP stats", Font_7x10);
        ssd1306_SetCursor(80, 0);
        		    if (ip_assigned){
     			    ssd1306_WriteString("dhcp", Font_7x10);
     			    } else {
     			    if (linkStatus == PHY_LINK_ON)
     			    ssd1306_WriteString("stat", Font_7x10);
     			    else {
     			    	ssd1306_WriteString("discon", Font_7x10);
     			    }
     			    }	uint32_t temp_sn = net_info.sn[0]<<24|net_info.sn[1]<<16|net_info.sn[2]<<8|net_info.sn[3];
	    char sn_slashed = 0;
	    ssd1306_SetColor(White);
   	  	ssd1306_SetCursor(0, 11);

	    for( char i =0; i < 32;i++){sn_slashed += (temp_sn>>i)&0x00000001;}
	  	sprintf(line,"%03d.%03d.%03d.%03d/%02d", net_info.ip[0],net_info.ip[1],net_info.ip[2],net_info.ip[3],sn_slashed);
	    ssd1306_WriteString(line, Font_7x8);
	    ssd1306_SetCursor(0, 22);
	    sprintf(line,"lease:%li", getDHCPLeasetime());
	  	ssd1306_WriteString(line, Font_7x8);

	break;
	case MENU_DNS:
			ssd1306_SetCursor(8, 0);
			ssd1306_WriteString("DNS stats", Font_7x10);
		    ssd1306_SetCursor(0, 11);
			sprintf(line,"dns%3d.%3d.%3d.%3d", net_info.dns[0],net_info.dns[1],net_info.dns[2],net_info.dns[3]);
		    ssd1306_WriteString(line, Font_7x8);
		    ssd1306_SetCursor(0, 22);
		    sprintf(line,"gw:%03d.%03d.%03d.%03d", net_info.gw[0],net_info.gw[1],net_info.gw[2],net_info.gw[3]);
		    ssd1306_WriteString(line, Font_7x8);

		break;
	case MENU_TELNET:
			ssd1306_SetCursor(8, 0);
			ssd1306_WriteString("TCP Server", Font_7x10);
		    ssd1306_SetCursor(80, 0);
			sprintf(line,"(%5d)", telnetConfigServer_getPort());
		    ssd1306_WriteString(line, Font_7x8);
		    ssd1306_SetCursor(0, 11);
		    switch (telnetConfigServer_SocketStatus()){
		    case SOCK_INIT:
		    	ssd1306_WriteString("Status:INIT", Font_7x8);
		    	break;
		    case SOCK_LISTEN:
		    	ssd1306_WriteString("Status:LISTEN", Font_7x8);
		    	break;
		    case SOCK_ESTABLISHED:
		    	ssd1306_WriteString("Status:ESTABLISHED", Font_7x8);
		    	break;
		    case SOCK_CLOSE_WAIT:
		   		    	ssd1306_WriteString("Status:CLOSE_WAIT", Font_7x8);
		   		    	break;
		    case SOCK_CLOSED:
		   		    	ssd1306_WriteString("Status:CLOSED", Font_7x8);
		   		    	break;
		   		    }
		    ssd1306_SetCursor(0, 22);
		    switch (telnetConfigServer_SocketProcess()){
		    case SOCKET_IDLE: ssd1306_WriteString("Process:IDLE", Font_7x8); break;
		    case SOCKET_RECEIVE: ssd1306_WriteString("Process:RECEIVE", Font_7x8); break;
		    case SOCKET_TRANSMIT: ssd1306_WriteString("Process:TRANSMIT", Font_7x8); 	break;
		    }


		break;

	case MENU_IDLE:
	default:
		   if (linkStatus == PHY_LINK_ON){
		   	   	ssd1306_DrawBitmap(0, 0, 13, 8, icon_13x8_link_conn);
		        } else {
		        ssd1306_DrawBitmap(0, 0, 13, 8, icon_13x8_link_empty);
		        }
			    if (ip_assigned){
			    	ssd1306_DrawBitmap(15, 0, 12, 8, icon_12x8_dhcp);
			    } else {
			    if (linkStatus == PHY_LINK_ON)
			    	ssd1306_DrawBitmap(15, 0, 12, 8, icon_12x8_static);
			    else {
			    	ssd1306_DrawBitmap(15, 0, 12, 8, icon_12x8_discon);
			    }
			    }
			    switch (telnetConfigServer_SocketStatus()){
			    case SOCK_INIT:
			    	sprintf(TelnetStatus,"Telnet init");
			    break;
			    case SOCK_LISTEN:
			    	sprintf(TelnetStatus,"Telnet listen");
			    	break;
			    case SOCK_ESTABLISHED:
			    	sprintf(TelnetStatus,"Client connected");
			    	ssd1306_DrawBitmap(30, 0, 12, 8, icon_12x8_connector);break;
			    case SOCK_CLOSE_WAIT:
			    			    	sprintf(TelnetStatus,"Client disconnect");
			    			    	break;
			    case SOCK_CLOSED:
			    			    	sprintf(TelnetStatus," ");
			    			    	break;
			    			    }
			    switch (telnetConfigServer_SocketProcess()){
			    case SOCKET_IDLE:
			    			   			    	ssd1306_DrawBitmap(44, 0, 12, 8, icon_12x8_TRX);
			    			   			    	break;
			    case SOCKET_RECEIVE:
			    			   			    	ssd1306_DrawBitmap(44, 0, 12, 8, icon_12x8_RX);
			    			   			    	break;
			    case SOCKET_TRANSMIT:
			    			   			    	ssd1306_DrawBitmap(44, 0, 12, 8, icon_12x8_TX);
			    			   			    	break;
			    			   			    }

			    ssd1306_SetCursor(00, 11);
			    ssd1306_WriteString(TelnetStatus, Font_7x8);
			    ssd1306_SetCursor(00, 22);
			    ssd1306_WriteString(status, Font_7x8);
	}

	ssd1306_UpdateScreen();
}
void w5500_init() {
	//registration callbacks
	 reg_wizchip_cs_cbfunc(W5500_Select, W5500_Unselect);
	    reg_wizchip_spi_cbfunc(W5500_ReadByte, W5500_WriteByte);
	    reg_wizchip_spiburst_cbfunc(W5500_ReadBuff, W5500_WriteBuff);
	    draw_menu(menu_idx);
	    //Calling wizchip_init()...
	       uint8_t rx_tx_buff_sizes[] = {2, 2, 2, 2, 2, 2, 2, 2};
	       wizchip_init(rx_tx_buff_sizes, rx_tx_buff_sizes);
	       draw_menu(menu_idx);
	       setSHAR(net_info.mac);
	       getSHAR(net_info.mac);
	       draw_menu(menu_idx);
	       reg_dhcp_cbfunc(
	       			              Callback_IPAssigned,
	       			              Callback_IPAssigned,
	       			              Callback_IPConflict
	       );

          sprintf(status,"Init complete");

}

void INTkeyMenu_handler() {menu_idx= 0;}
void INTkeyUp_handler() {menu_idx++; menu_idx %=5;}
void INTkeyDown_handler() {menu_idx +=4;menu_idx %=5;}
void INTW5500_Handler() {  sprintf(status,"INT W5500"); telnetConfigServer_InterruptCallback();}


/* USER CODE END 0 */

/**
  * @brief  The application entry point.
  * @retval int
  */
int main(void)
{
  /* USER CODE BEGIN 1 */

  /* USER CODE END 1 */

  /* MCU Configuration--------------------------------------------------------*/

  /* Reset of all peripherals, Initializes the Flash interface and the Systick. */
  HAL_Init();

  /* USER CODE BEGIN Init */

  /* USER CODE END Init */

  /* Configure the system clock */
  SystemClock_Config();

  /* USER CODE BEGIN SysInit */

  /* USER CODE END SysInit */

  /* Initialize all configured peripherals */
  MX_GPIO_Init();
  MX_DMA_Init();
  MX_I2C1_Init();
  MX_SPI1_Init();
  MX_TIM1_Init();
  /* USER CODE BEGIN 2 */
  HAL_Delay(500);
  ssd1306_Init();
  ssd1306_FlipScreenVertically();
  ssd1306_UpdateScreen();
  sprintf(status,"Init display");
  draw_menu(0);
  HAL_SPI_Init(&hspi1);
  HAL_GPIO_WritePin(W5500_RESET_GPIO_Port, W5500_RESET_Pin, GPIO_PIN_RESET);
  HAL_Delay(1);
  HAL_GPIO_WritePin(W5500_RESET_GPIO_Port, W5500_RESET_Pin, GPIO_PIN_SET);
  sprintf(status,"Reset W5500");
  draw_menu(0);

  sprintf(status,"Init W5500");

  w5500_init();
  linkParametersStorage(&params,PARAMS_COUNT);
  initParameters();

  /* USER CODE END 2 */

  /* Infinite loop */
  /* USER CODE BEGIN WHILE */
  while (1)
  {
	 // loop();
	  HAL_Delay(100);
	  draw_menu(menu_idx);
	  linkStatus = wizphy_getphylink();
	  wizphy_getphystat(&PhyStatus);
	  if (linkStatus != oldlinkStatus  && linkStatus == PHY_LINK_OFF) { // Кабель подключили
	       sprintf(status,"Cable disconnected");
			       draw_menu(0);
			       telnetConfigServer_SocketClose();
			       ip_assigned = 0;

	  }
	  if (linkStatus != oldlinkStatus  && linkStatus == PHY_LINK_ON) { // Кабель подключили
		  sprintf(status,"Cable connected");
		  draw_menu(0);
		  net_info.dhcp = NETINFO_DHCP;
		  wizchip_setnetinfo(&net_info);

		    DHCP_init(DHCP_SOCKET, dhcp_buffer);
			       sprintf(status,"DHCP discovering");
			       draw_menu(0);
			       uint32_t ctr = 10000;
			        while((!ip_assigned) && (ctr > 0)) {
			            DHCP_run();
			            ctr--;
			            HAL_Delay(1);
			        }
			        if(!ip_assigned && retries < 5) {
			        	draw_menu(0);
			        	retries++;
			        	linkStatus = PHY_LINK_OFF; //инициируем retry
					       sprintf(status,"DHCP not found");
					       draw_menu(0);
					       HAL_Delay(300);
					 }
			        else {
			        	if(ip_assigned){
			        	retries = 0;
                   sprintf(status,"Assign network");
			       draw_menu(0);

			       getIPfromDHCP(net_info.ip);
			       getGWfromDHCP(net_info.gw);
			       getSNfromDHCP(net_info.sn);
			       sprintf(status,"Find DNS server");
			       draw_menu(0);

			       getDNSfromDHCP(net_info.dns);
			       } else {net_info.dhcp = NETINFO_STATIC;}
			       sprintf(status,"Store Net config");
			       draw_menu(0);
			       wizchip_setnetinfo(&net_info);
			       sprintf(status,"Ready");
			       			       draw_menu(0);
			       			    telnetConfigServer_Init(3324);
			       			    telnetConfigServer_SocketOpen();
			       }
	  }
	  oldlinkStatus = linkStatus;
	  telnetConfigServer_InterruptCallback();
    /* USER CODE END WHILE */

    /* USER CODE BEGIN 3 */
  }
  /* USER CODE END 3 */
}

/**
  * @brief System Clock Configuration
  * @retval None
  */
void SystemClock_Config(void)
{
  RCC_OscInitTypeDef RCC_OscInitStruct = {0};
  RCC_ClkInitTypeDef RCC_ClkInitStruct = {0};

  /** Initializes the RCC Oscillators according to the specified parameters
  * in the RCC_OscInitTypeDef structure.
  */
  RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSE;
  RCC_OscInitStruct.HSEState = RCC_HSE_ON;
  RCC_OscInitStruct.HSEPredivValue = RCC_HSE_PREDIV_DIV2;
  RCC_OscInitStruct.HSIState = RCC_HSI_ON;
  RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
  RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSE;
  RCC_OscInitStruct.PLL.PLLMUL = RCC_PLL_MUL16;
  if (HAL_RCC_OscConfig(&RCC_OscInitStruct) != HAL_OK)
  {
    Error_Handler();
  }
  /** Initializes the CPU, AHB and APB buses clocks
  */
  RCC_ClkInitStruct.ClockType = RCC_CLOCKTYPE_HCLK|RCC_CLOCKTYPE_SYSCLK
                              |RCC_CLOCKTYPE_PCLK1|RCC_CLOCKTYPE_PCLK2;
  RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
  RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV1;
  RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV2;
  RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV1;

  if (HAL_RCC_ClockConfig(&RCC_ClkInitStruct, FLASH_LATENCY_2) != HAL_OK)
  {
    Error_Handler();
  }
  /** Enables the Clock Security System
  */
  HAL_RCC_EnableCSS();
}

/**
  * @brief I2C1 Initialization Function
  * @param None
  * @retval None
  */
static void MX_I2C1_Init(void)
{

  /* USER CODE BEGIN I2C1_Init 0 */

  /* USER CODE END I2C1_Init 0 */

  /* USER CODE BEGIN I2C1_Init 1 */

  /* USER CODE END I2C1_Init 1 */
  hi2c1.Instance = I2C1;
  hi2c1.Init.ClockSpeed = 400000;
  hi2c1.Init.DutyCycle = I2C_DUTYCYCLE_2;
  hi2c1.Init.OwnAddress1 = 0;
  hi2c1.Init.AddressingMode = I2C_ADDRESSINGMODE_7BIT;
  hi2c1.Init.DualAddressMode = I2C_DUALADDRESS_DISABLE;
  hi2c1.Init.OwnAddress2 = 0;
  hi2c1.Init.GeneralCallMode = I2C_GENERALCALL_DISABLE;
  hi2c1.Init.NoStretchMode = I2C_NOSTRETCH_ENABLE;
  if (HAL_I2C_Init(&hi2c1) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN I2C1_Init 2 */

  /* USER CODE END I2C1_Init 2 */

}

/**
  * @brief SPI1 Initialization Function
  * @param None
  * @retval None
  */
static void MX_SPI1_Init(void)
{

  /* USER CODE BEGIN SPI1_Init 0 */

  /* USER CODE END SPI1_Init 0 */

  /* USER CODE BEGIN SPI1_Init 1 */

  /* USER CODE END SPI1_Init 1 */
  /* SPI1 parameter configuration*/
  hspi1.Instance = SPI1;
  hspi1.Init.Mode = SPI_MODE_MASTER;
  hspi1.Init.Direction = SPI_DIRECTION_2LINES;
  hspi1.Init.DataSize = SPI_DATASIZE_8BIT;
  hspi1.Init.CLKPolarity = SPI_POLARITY_LOW;
  hspi1.Init.CLKPhase = SPI_PHASE_1EDGE;
  hspi1.Init.NSS = SPI_NSS_SOFT;
  hspi1.Init.BaudRatePrescaler = SPI_BAUDRATEPRESCALER_4;
  hspi1.Init.FirstBit = SPI_FIRSTBIT_MSB;
  hspi1.Init.TIMode = SPI_TIMODE_DISABLE;
  hspi1.Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
  hspi1.Init.CRCPolynomial = 10;
  if (HAL_SPI_Init(&hspi1) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN SPI1_Init 2 */

  /* USER CODE END SPI1_Init 2 */

}

/**
  * @brief TIM1 Initialization Function
  * @param None
  * @retval None
  */
static void MX_TIM1_Init(void)
{

  /* USER CODE BEGIN TIM1_Init 0 */

  /* USER CODE END TIM1_Init 0 */

  TIM_ClockConfigTypeDef sClockSourceConfig = {0};
  TIM_MasterConfigTypeDef sMasterConfig = {0};
  TIM_OC_InitTypeDef sConfigOC = {0};
  TIM_BreakDeadTimeConfigTypeDef sBreakDeadTimeConfig = {0};

  /* USER CODE BEGIN TIM1_Init 1 */

  /* USER CODE END TIM1_Init 1 */
  htim1.Instance = TIM1;
  htim1.Init.Prescaler = 0;
  htim1.Init.CounterMode = TIM_COUNTERMODE_UP;
  htim1.Init.Period = 65535;
  htim1.Init.ClockDivision = TIM_CLOCKDIVISION_DIV1;
  htim1.Init.RepetitionCounter = 0;
  htim1.Init.AutoReloadPreload = TIM_AUTORELOAD_PRELOAD_DISABLE;
  if (HAL_TIM_Base_Init(&htim1) != HAL_OK)
  {
    Error_Handler();
  }
  sClockSourceConfig.ClockSource = TIM_CLOCKSOURCE_INTERNAL;
  if (HAL_TIM_ConfigClockSource(&htim1, &sClockSourceConfig) != HAL_OK)
  {
    Error_Handler();
  }
  if (HAL_TIM_OC_Init(&htim1) != HAL_OK)
  {
    Error_Handler();
  }
  sMasterConfig.MasterOutputTrigger = TIM_TRGO_OC1REF;
  sMasterConfig.MasterSlaveMode = TIM_MASTERSLAVEMODE_DISABLE;
  if (HAL_TIMEx_MasterConfigSynchronization(&htim1, &sMasterConfig) != HAL_OK)
  {
    Error_Handler();
  }
  sConfigOC.OCMode = TIM_OCMODE_TIMING;
  sConfigOC.Pulse = 0;
  sConfigOC.OCPolarity = TIM_OCPOLARITY_HIGH;
  sConfigOC.OCNPolarity = TIM_OCNPOLARITY_HIGH;
  sConfigOC.OCFastMode = TIM_OCFAST_DISABLE;
  sConfigOC.OCIdleState = TIM_OCIDLESTATE_RESET;
  sConfigOC.OCNIdleState = TIM_OCNIDLESTATE_RESET;
  if (HAL_TIM_OC_ConfigChannel(&htim1, &sConfigOC, TIM_CHANNEL_1) != HAL_OK)
  {
    Error_Handler();
  }
  sBreakDeadTimeConfig.OffStateRunMode = TIM_OSSR_DISABLE;
  sBreakDeadTimeConfig.OffStateIDLEMode = TIM_OSSI_DISABLE;
  sBreakDeadTimeConfig.LockLevel = TIM_LOCKLEVEL_OFF;
  sBreakDeadTimeConfig.DeadTime = 0;
  sBreakDeadTimeConfig.BreakState = TIM_BREAK_DISABLE;
  sBreakDeadTimeConfig.BreakPolarity = TIM_BREAKPOLARITY_HIGH;
  sBreakDeadTimeConfig.AutomaticOutput = TIM_AUTOMATICOUTPUT_DISABLE;
  if (HAL_TIMEx_ConfigBreakDeadTime(&htim1, &sBreakDeadTimeConfig) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN TIM1_Init 2 */

  /* USER CODE END TIM1_Init 2 */

}

/**
  * Enable DMA controller clock
  */
static void MX_DMA_Init(void)
{

  /* DMA controller clock enable */
  __HAL_RCC_DMA1_CLK_ENABLE();

  /* DMA interrupt init */
  /* DMA1_Channel6_IRQn interrupt configuration */
  HAL_NVIC_SetPriority(DMA1_Channel6_IRQn, 0, 0);
  HAL_NVIC_EnableIRQ(DMA1_Channel6_IRQn);

}

/**
  * @brief GPIO Initialization Function
  * @param None
  * @retval None
  */
static void MX_GPIO_Init(void)
{
  GPIO_InitTypeDef GPIO_InitStruct = {0};

  /* GPIO Ports Clock Enable */
  __HAL_RCC_GPIOC_CLK_ENABLE();
  __HAL_RCC_GPIOA_CLK_ENABLE();
  __HAL_RCC_GPIOB_CLK_ENABLE();
  __HAL_RCC_GPIOD_CLK_ENABLE();

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(GPIOC, LED_Pin|W5500_RESET_Pin, GPIO_PIN_RESET);

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(W5500_CS_GPIO_Port, W5500_CS_Pin, GPIO_PIN_RESET);

  /*Configure GPIO pins : LED_Pin W5500_RESET_Pin */
  GPIO_InitStruct.Pin = LED_Pin|W5500_RESET_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Pull = GPIO_PULLUP;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(GPIOC, &GPIO_InitStruct);

  /*Configure GPIO pin : KEY_MENU_TOGGLE_Pin */
  GPIO_InitStruct.Pin = KEY_MENU_TOGGLE_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_IT_RISING;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  HAL_GPIO_Init(KEY_MENU_TOGGLE_GPIO_Port, &GPIO_InitStruct);

  /*Configure GPIO pin : W5500_CS_Pin */
  GPIO_InitStruct.Pin = W5500_CS_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(W5500_CS_GPIO_Port, &GPIO_InitStruct);

  /*Configure GPIO pins : KEY_DN_Pin KEY_UP_Pin */
  GPIO_InitStruct.Pin = KEY_DN_Pin|KEY_UP_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_IT_RISING;
  GPIO_InitStruct.Pull = GPIO_PULLUP;
  HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);

  /*Configure GPIO pin : W5500_INT_Pin */
  GPIO_InitStruct.Pin = W5500_INT_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_IT_RISING;
  GPIO_InitStruct.Pull = GPIO_PULLUP;
  HAL_GPIO_Init(W5500_INT_GPIO_Port, &GPIO_InitStruct);

  /* EXTI interrupt init*/
  HAL_NVIC_SetPriority(EXTI0_IRQn, 0, 0);
  HAL_NVIC_EnableIRQ(EXTI0_IRQn);

  HAL_NVIC_SetPriority(EXTI1_IRQn, 0, 0);
  HAL_NVIC_EnableIRQ(EXTI1_IRQn);

  HAL_NVIC_SetPriority(EXTI9_5_IRQn, 0, 0);
  HAL_NVIC_EnableIRQ(EXTI9_5_IRQn);

  HAL_NVIC_SetPriority(EXTI15_10_IRQn, 0, 0);
  HAL_NVIC_EnableIRQ(EXTI15_10_IRQn);

}

/* USER CODE BEGIN 4 */

/* USER CODE END 4 */

/**
  * @brief  This function is executed in case of error occurrence.
  * @retval None
  */
void Error_Handler(void)
{
  /* USER CODE BEGIN Error_Handler_Debug */
  /* User can add his own implementation to report the HAL error return state */
  __disable_irq();
  while (1)
  {
  }
  /* USER CODE END Error_Handler_Debug */
}

#ifdef  USE_FULL_ASSERT
/**
  * @brief  Reports the name of the source file and the source line number
  *         where the assert_param error has occurred.
  * @param  file: pointer to the source file name
  * @param  line: assert_param error line source number
  * @retval None
  */
void assert_failed(uint8_t *file, uint32_t line)
{
  /* USER CODE BEGIN 6 */
  /* User can add his own implementation to report the file name and line number,
     ex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */
  /* USER CODE END 6 */
}
#endif /* USE_FULL_ASSERT */

