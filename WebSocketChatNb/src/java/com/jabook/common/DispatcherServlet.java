package com.jabook.common;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
import java.io.IOException;

import java.util.HashMap;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletConfig;

/**
 *
 * @author sshtel
 */
/*
Library dependency problem:
These library version should match together.

reflections : 0.9.8
guava : 11.0.2
 */
public class DispatcherServlet extends HttpServlet {

    HashMap<String, Object> methodContainer = new HashMap<>();
    HashMap<Class, Object> controllerContainer = new HashMap<>();

    public static String resultPage = "/jsp/board/resultPage.jsp";

    @Override
    public void init(ServletConfig config) {
        //TODO
    }

    @Override
    public void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        process(request, response);
    }

    @Override
    public void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        process(request, response);
    }

    private void process(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        String cmd = request.getRequestURI();
        System.out.println("cmd:" + cmd + " request.getContextPath():" + request.getContextPath());

        cmd = cmd.substring(request.getContextPath().length());

        cmd = cmd.replace(".do", ".jsp");
        RequestDispatcher dispatcher = request.getRequestDispatcher(cmd);
        dispatcher.forward(request, response);

    }
}
