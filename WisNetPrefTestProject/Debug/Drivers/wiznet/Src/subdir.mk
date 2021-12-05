################################################################################
# Automatically-generated file. Do not edit!
# Toolchain: GNU Tools for STM32 (9-2020-q2-update)
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
C_SRCS += \
../Drivers/wiznet/Src/dhcp.c \
../Drivers/wiznet/Src/dns.c \
../Drivers/wiznet/Src/httpParser.c \
../Drivers/wiznet/Src/httpServer.c \
../Drivers/wiznet/Src/httpUtil.c \
../Drivers/wiznet/Src/parameters.c \
../Drivers/wiznet/Src/socket.c \
../Drivers/wiznet/Src/telnetConfigServer.c \
../Drivers/wiznet/Src/w5500.c \
../Drivers/wiznet/Src/wizchip_conf.c 

OBJS += \
./Drivers/wiznet/Src/dhcp.o \
./Drivers/wiznet/Src/dns.o \
./Drivers/wiznet/Src/httpParser.o \
./Drivers/wiznet/Src/httpServer.o \
./Drivers/wiznet/Src/httpUtil.o \
./Drivers/wiznet/Src/parameters.o \
./Drivers/wiznet/Src/socket.o \
./Drivers/wiznet/Src/telnetConfigServer.o \
./Drivers/wiznet/Src/w5500.o \
./Drivers/wiznet/Src/wizchip_conf.o 

C_DEPS += \
./Drivers/wiznet/Src/dhcp.d \
./Drivers/wiznet/Src/dns.d \
./Drivers/wiznet/Src/httpParser.d \
./Drivers/wiznet/Src/httpServer.d \
./Drivers/wiznet/Src/httpUtil.d \
./Drivers/wiznet/Src/parameters.d \
./Drivers/wiznet/Src/socket.d \
./Drivers/wiznet/Src/telnetConfigServer.d \
./Drivers/wiznet/Src/w5500.d \
./Drivers/wiznet/Src/wizchip_conf.d 


# Each subdirectory must supply rules for building sources it contributes
Drivers/wiznet/Src/%.o: ../Drivers/wiznet/Src/%.c Drivers/wiznet/Src/subdir.mk
	arm-none-eabi-gcc "$<" -mcpu=cortex-m3 -std=gnu11 -g3 -DDEBUG -DUSE_HAL_DRIVER -DSTM32F103xE -c -I../Core/Inc -I../Drivers/STM32F1xx_HAL_Driver/Inc -I../Drivers/STM32F1xx_HAL_Driver/Inc/Legacy -I../Drivers/CMSIS/Device/ST/STM32F1xx/Include -I../Drivers/CMSIS/Include -I../Drivers/wiznet/Inc -O0 -ffunction-sections -fdata-sections -Wall -fstack-usage -MMD -MP -MF"$(@:%.o=%.d)" -MT"$@" --specs=nano.specs -mfloat-abi=soft -mthumb -o "$@"

clean: clean-Drivers-2f-wiznet-2f-Src

clean-Drivers-2f-wiznet-2f-Src:
	-$(RM) ./Drivers/wiznet/Src/dhcp.d ./Drivers/wiznet/Src/dhcp.o ./Drivers/wiznet/Src/dns.d ./Drivers/wiznet/Src/dns.o ./Drivers/wiznet/Src/httpParser.d ./Drivers/wiznet/Src/httpParser.o ./Drivers/wiznet/Src/httpServer.d ./Drivers/wiznet/Src/httpServer.o ./Drivers/wiznet/Src/httpUtil.d ./Drivers/wiznet/Src/httpUtil.o ./Drivers/wiznet/Src/parameters.d ./Drivers/wiznet/Src/parameters.o ./Drivers/wiznet/Src/socket.d ./Drivers/wiznet/Src/socket.o ./Drivers/wiznet/Src/telnetConfigServer.d ./Drivers/wiznet/Src/telnetConfigServer.o ./Drivers/wiznet/Src/w5500.d ./Drivers/wiznet/Src/w5500.o ./Drivers/wiznet/Src/wizchip_conf.d ./Drivers/wiznet/Src/wizchip_conf.o

.PHONY: clean-Drivers-2f-wiznet-2f-Src

