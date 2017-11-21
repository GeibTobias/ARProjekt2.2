package com.arvr;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import com.arvr.utils.ListManager;

@Configuration
public class AppConfig {
	
    @Bean
    public ListManager transferService() {
        return new ListManager();
    }
}