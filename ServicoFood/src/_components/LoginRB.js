//import liraries
import React, { Component } from 'react';

import { Container, Item, Input, Header, Title, Content, Footer, FooterTab, Button, Left, Right, Body, Icon, Text, ListItem, CheckBox 
,Thumbnail

} from 'native-base';

// create a component
class LoginRB extends Component {
    render() {
        return (
        <Container>
            <Header>
                <Left>
                    <Button transparent>
                    <Icon name='menu' />
                    </Button>
                </Left>
                <Body>
                    <Title>Header</Title>
                </Body>
                <Right />
            </Header>
            <Content>
                <Thumbnail source={require('../_images/logo@2.png')} />

                <Text>
                    This is Content Section
                </Text>
                <Button light><Text> Light </Text></Button>
                <Button primary><Text> Primary </Text></Button>
                <Button success><Text> Success </Text></Button>
                <Button info><Text> Info </Text></Button>
                <Button warning><Text> Warning </Text></Button>
                <Button danger><Text> Danger </Text></Button>
                <Button dark full><Text> Dark </Text></Button>
                <Button iconLeft light>
                    <Icon name='arrow-back' />
                    <Text>Back</Text>
                </Button>
                <Button iconLeft dark>
                    <Icon name='cog' />
                    <Text>Settings</Text>
                </Button>
                <Button block light>
                    <Text>Light</Text>
                </Button>
                <ListItem>
                    <CheckBox checked={true} />
                    <Body>
                        <Text>Daily Stand Up</Text>
                    </Body>
                </ListItem>

                <Item success>
                    <Icon active name='camera' />
                    <Input placeholder='Icon Textbox'/>
                </Item>


            </Content>            
            <Footer>
                <FooterTab>
                    <Button vertical>
                        <Icon name="apps" />
                        <Text>Apps</Text>
                    </Button>
                    <Button vertical>
                        <Icon name="camera" />
                        <Text>Camera</Text>
                    </Button>
                    <Button vertical active>
                        <Icon active name="navigate" />
                        <Text>Navigate</Text>
                    </Button>
                    <Button vertical>
                        <Icon name="person" />
                        <Text>Contact</Text>
                    </Button>
                </FooterTab>
            </Footer>

        </Container>
        );
    }
}

//make this component available to the app
export default LoginRB;
