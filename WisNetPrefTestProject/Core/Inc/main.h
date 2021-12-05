/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.h
  * @brief          : Header for main.c file.
  *                   This file contains the common defines of the application.
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

/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __MAIN_H
#define __MAIN_H

#ifdef __cplusplus
extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include "stm32f1xx_hal.h"

/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */

/* USER CODE END Includes */

/* Exported types ------------------------------------------------------------*/
/* USER CODE BEGIN ET */

/* USER CODE END ET */

/* Exported constants --------------------------------------------------------*/
/* USER CODE BEGIN EC */

/* USER CODE END EC */

/* Exported macro ------------------------------------------------------------*/
/* USER CODE BEGIN EM */

/* USER CODE END EM */

/* Exported functions prototypes ---------------------------------------------*/
void Error_Handler(void);

/* USER CODE BEGIN EFP */


/* USER CODE END EFP */

/* Private defines -----------------------------------------------------------*/
#define LED_Pin GPIO_PIN_13
#define LED_GPIO_Port GPIOC
#define KEY_MENU_TOGGLE_Pin GPIO_PIN_0
#define KEY_MENU_TOGGLE_GPIO_Port GPIOC
#define KEY_MENU_TOGGLE_EXTI_IRQn EXTI0_IRQn
#define W5500_CS_Pin GPIO_PIN_4
#define W5500_CS_GPIO_Port GPIOA
#define W5500_SCK_Pin GPIO_PIN_5
#define W5500_SCK_GPIO_Port GPIOA
#define W5500_MI_Pin GPIO_PIN_6
#define W5500_MI_GPIO_Port GPIOA
#define W5500_MO_Pin GPIO_PIN_7
#define W5500_MO_GPIO_Port GPIOA
#define KEY_DN_Pin GPIO_PIN_1
#define KEY_DN_GPIO_Port GPIOB
#define KEY_DN_EXTI_IRQn EXTI1_IRQn
#define KEY_UP_Pin GPIO_PIN_15
#define KEY_UP_GPIO_Port GPIOB
#define KEY_UP_EXTI_IRQn EXTI15_10_IRQn
#define W5500_RESET_Pin GPIO_PIN_11
#define W5500_RESET_GPIO_Port GPIOC
#define W5500_INT_Pin GPIO_PIN_6
#define W5500_INT_GPIO_Port GPIOD
#define W5500_INT_EXTI_IRQn EXTI9_5_IRQn
/* USER CODE BEGIN Private defines */

/* USER CODE END Private defines */

#ifdef __cplusplus
}
#endif

#endif /* __MAIN_H */
