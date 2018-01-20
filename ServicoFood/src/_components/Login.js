//import liraries
import React, { Component } from 'react';
import { View, Text, Image, StyleSheet, KeyboardAvoidingView } from 'react-native';
import LoginForm from './LoginForm';

// create a component
class Login extends Component {
    render() {
        return (
            <KeyboardAvoidingView behavior="padding" style={styles.container}>
                <View style={styles.logoContainer}>
                     <Image style={styles.logo} source={require('../_images/logo@2.png')} /> 
                     <Text style={styles.title}>Julia maior fonte de dados de imóveis do Brasil</Text>
                     
                </View>
                <View style={styles.formContainer}>
                    <LoginForm />
                </View>
            </KeyboardAvoidingView>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#3498db'
    },
    logoContainer: {
        alignItems: 'center',
        flexGrow: 1,
        justifyContent: 'center'
    },
    logo: {
    },
    title: {
        width: 300,
        color: '#FFF',
        marginTop: 10,
        textAlign: 'center' 
    }
  });

//make this component available to the app
export default Login;
