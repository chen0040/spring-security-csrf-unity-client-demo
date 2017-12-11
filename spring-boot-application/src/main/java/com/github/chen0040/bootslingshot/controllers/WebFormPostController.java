package com.github.chen0040.bootslingshot.controllers;

import com.alibaba.fastjson.JSON;
import com.github.chen0040.bootslingshot.components.SpringRequestHelper;
import com.github.chen0040.bootslingshot.models.LoginObj;
import com.github.chen0040.bootslingshot.models.SpringIdentity;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.web.authentication.WebAuthenticationDetails;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import java.util.Map;

@Controller
public class WebFormPostController {

    @Autowired
    private SpringRequestHelper requestHelper;

    @Autowired
    private AuthenticationManager authenticationManager;

    private static final Logger logger = LoggerFactory.getLogger(WebFormPostController.class);

    @RequestMapping(value="/erp/login-api-form-post", method = RequestMethod.GET)
    public @ResponseBody Map<String, String> getLoginApiJson(HttpServletRequest request) {
        return requestHelper.getTokenInfo(request);
    }

    @RequestMapping(value="/erp/login-api-form-post", method = RequestMethod.POST)
    public @ResponseBody SpringIdentity postLoginApiJson(@RequestParam("username") String username, @RequestParam("password") String password, HttpServletRequest request) {

        logger.info("POST[login]");
        UsernamePasswordAuthenticationToken token = new UsernamePasswordAuthenticationToken(username, password);

        // generate session if does not exists
        request.getSession();

        token.setDetails(new WebAuthenticationDetails(request));

        Authentication authentication = authenticationManager.authenticate(token);

        SecurityContextHolder.getContext().setAuthentication(authentication);

        SpringIdentity identity = new SpringIdentity();
        identity.setAuthenticated(authentication.isAuthenticated());
        identity.setUsername(username);

        logger.info("login post: {}", JSON.toJSONString(identity));

        return identity;
    }
}
