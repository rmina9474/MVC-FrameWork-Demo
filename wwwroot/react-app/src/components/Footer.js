import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

const FooterContainer = styled.footer`
  background-color: #fff;
  padding: 0;
  border-top: 1px solid #e5e5e5;
  font-size: 14px;
`;

const FooterSection = styled.div`
  max-width: 1440px;
  margin: 0 auto;
  padding: 0 40px;
  
  @media (max-width: 768px) {
    padding: 0 20px;
  }
`;

const FooterNav = styled.div`
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  padding: 40px 0;
  
  @media (max-width: 996px) {
    grid-template-columns: 1fr;
    padding: 0;
  }
`;

const FooterColumn = styled.div`
  @media (max-width: 996px) {
    border-bottom: 1px solid #e5e5e5;
  }
`;

const FooterHeading = styled.h2`
  font-size: 16px;
  font-weight: 600;
  margin-bottom: 24px;
  cursor: pointer;
  position: relative;
  
  @media (max-width: 996px) {
    margin: 0;
    padding: 16px 0;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
  
  &::after {
    content: '';
    display: none;
    width: 10px;
    height: 10px;
    border-right: 2px solid #000;
    border-bottom: 2px solid #000;
    transform: ${({ expanded }) => expanded ? 'rotate(-135deg)' : 'rotate(45deg)'};
    transition: transform 0.3s ease;
    
    @media (max-width: 996px) {
      display: block;
    }
  }
`;

const FooterLinks = styled.ul`
  list-style: none;
  padding: 0;
  margin: 0;
  transition: max-height 0.3s ease;
  
  @media (max-width: 996px) {
    max-height: ${({ expanded }) => expanded ? '1000px' : '0'};
    overflow: hidden;
    margin-bottom: ${({ expanded }) => expanded ? '16px' : '0'};
  }
`;

const FooterLink = styled.li`
  margin-bottom: 20px;
  
  a {
    color: rgba(0, 0, 0, 0.7);
    font-weight: 400;
    
    &:hover {
      color: var(--starbucks-black);
    }
  }
`;

const SocialSection = styled.div`
  padding: 32px 0;
  border-top: 1px solid #e5e5e5;
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
`;

const SocialIcons = styled.div`
  display: flex;
  gap: 16px;
`;

const SocialIcon = styled.a`
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background-color: var(--starbucks-black);
  color: white;
  transition: all 0.3s ease;
  
  &:hover {
    transform: translateY(-3px);
    background-color: var(--starbucks-green);
  }
`;

const LegalSection = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
  padding: 24px 0;
  border-top: 1px solid #e5e5e5;
  
  a {
    color: rgba(0, 0, 0, 0.7);
    font-weight: 400;
    
    &:hover {
      color: var(--starbucks-black);
    }
  }
`;

const CopyrightText = styled.p`
  color: rgba(0, 0, 0, 0.7);
  margin: 24px 0;
  font-size: 12px;
`;

function Footer() {
  // For mobile accordion
  const [expandedSections, setExpandedSections] = useState({
    about: false,
    careers: false,
    social: false,
    business: false,
    order: false
  });

  const toggleSection = (section) => {
    setExpandedSections({
      ...expandedSections,
      [section]: !expandedSections[section]
    });
  };

  return (
    <FooterContainer>
      <FooterSection>
        <FooterNav>
          <FooterColumn>
            <FooterHeading 
              expanded={expandedSections.about} 
              onClick={() => toggleSection('about')}
            >
              About Us
            </FooterHeading>
            <FooterLinks expanded={expandedSections.about}>
              <FooterLink>
                <Link to="/about">Our Company</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/coffee">Our Coffee</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/stories">Stories and News</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/investor">Investor Relations</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/policies">Policies and Standards</Link>
              </FooterLink>
            </FooterLinks>
          </FooterColumn>

          <FooterColumn>
            <FooterHeading 
              expanded={expandedSections.careers} 
              onClick={() => toggleSection('careers')}
            >
              Careers
            </FooterHeading>
            <FooterLinks expanded={expandedSections.careers}>
              <FooterLink>
                <Link to="/careers">Culture and Values</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/careers/working">Inclusion, Diversity, and Equity</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/careers/benefits">College Achievement Plan</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/careers/jobs">U.S. Careers</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/careers/international">International Careers</Link>
              </FooterLink>
            </FooterLinks>
          </FooterColumn>

          <FooterColumn>
            <FooterHeading 
              expanded={expandedSections.social} 
              onClick={() => toggleSection('social')}
            >
              Social Impact
            </FooterHeading>
            <FooterLinks expanded={expandedSections.social}>
              <FooterLink>
                <Link to="/responsibility">People</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/responsibility/planet">Planet</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/responsibility/environmental">Environmental and Social Impact Reporting</Link>
              </FooterLink>
            </FooterLinks>
          </FooterColumn>

          <FooterColumn>
            <FooterHeading 
              expanded={expandedSections.business} 
              onClick={() => toggleSection('business')}
            >
              For Business Partners
            </FooterHeading>
            <FooterLinks expanded={expandedSections.business}>
              <FooterLink>
                <Link to="/suppliers">Landlord Support Center</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/suppliers/suppliers">Suppliers</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/corporate-gifting">Corporate Gift Card Sales</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/office-coffee">Office and Foodservice Coffee</Link>
              </FooterLink>
            </FooterLinks>
          </FooterColumn>

          <FooterColumn>
            <FooterHeading 
              expanded={expandedSections.order} 
              onClick={() => toggleSection('order')}
            >
              Order and Pickup
            </FooterHeading>
            <FooterLinks expanded={expandedSections.order}>
              <FooterLink>
                <Link to="/app">Order on the App</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/menu">Order on the Web</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/delivery">Delivery</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/ways-to-order">Order and Pickup Options</Link>
              </FooterLink>
              <FooterLink>
                <Link to="/gift-cards">Gift Cards</Link>
              </FooterLink>
            </FooterLinks>
          </FooterColumn>
        </FooterNav>

        <SocialSection>
          <SocialIcons>
            <SocialIcon href="https://www.spotify.com/starbucks" target="_blank" rel="noopener noreferrer">
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                <path d="M8 0a8 8 0 1 0 0 16A8 8 0 0 0 8 0zm3.669 11.538a.498.498 0 0 1-.686.165c-1.879-1.147-4.243-1.407-7.028-.77a.499.499 0 0 1-.222-.973c3.048-.696 5.662-.397 7.77.892a.5.5 0 0 1 .166.686zm.979-2.178a.624.624 0 0 1-.858.205c-2.15-1.321-5.428-1.704-7.972-.932a.625.625 0 0 1-.362-1.194c2.905-.881 6.517-.454 8.986 1.063a.624.624 0 0 1 .206.858zm.084-2.268C10.154 5.56 5.9 5.419 3.438 6.166a.748.748 0 1 1-.434-1.432c2.825-.857 7.523-.692 10.492 1.07a.747.747 0 1 1-.764 1.288z"/>
              </svg>
            </SocialIcon>
            <SocialIcon href="https://www.facebook.com/starbucks" target="_blank" rel="noopener noreferrer">
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                <path d="M16 8.049c0-4.446-3.582-8.05-8-8.05C3.58 0-.002 3.603-.002 8.05c0 4.017 2.926 7.347 6.75 7.951v-5.625h-2.03V8.05H6.75V6.275c0-2.017 1.195-3.131 3.022-3.131.876 0 1.791.157 1.791.157v1.98h-1.009c-.993 0-1.303.621-1.303 1.258v1.51h2.218l-.354 2.326H9.25V16c3.824-.604 6.75-3.934 6.75-7.951z"/>
              </svg>
            </SocialIcon>
            <SocialIcon href="https://www.pinterest.com/starbucks" target="_blank" rel="noopener noreferrer">
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                <path d="M8 0a8 8 0 0 0-2.915 15.452c-.07-.633-.134-1.606.027-2.297.146-.625.938-3.977.938-3.977s-.239-.479-.239-1.187c0-1.113.645-1.943 1.448-1.943.682 0 1.012.512 1.012 1.127 0 .686-.437 1.712-.663 2.663-.188.796.4 1.446 1.185 1.446 1.422 0 2.515-1.5 2.515-3.664 0-1.915-1.377-3.254-3.342-3.254-2.276 0-3.612 1.707-3.612 3.471 0 .688.265 1.425.595 1.826a.24.24 0 0 1 .056.23c-.061.252-.196.796-.222.907-.035.146-.116.177-.268.107-1-.465-1.624-1.926-1.624-3.1 0-2.523 1.834-4.84 5.286-4.84 2.775 0 4.932 1.977 4.932 4.62 0 2.757-1.739 4.976-4.151 4.976-.811 0-1.573-.421-1.834-.919l-.498 1.902c-.181.695-.669 1.566-.995 2.097A8 8 0 1 0 8 0z"/>
              </svg>
            </SocialIcon>
            <SocialIcon href="https://www.instagram.com/starbucks" target="_blank" rel="noopener noreferrer">
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                <path d="M8 0C5.829 0 5.556.01 4.703.048 3.85.088 3.269.222 2.76.42a3.917 3.917 0 0 0-1.417.923A3.927 3.927 0 0 0 .42 2.76C.222 3.268.087 3.85.048 4.7.01 5.555 0 5.827 0 8.001c0 2.172.01 2.444.048 3.297.04.852.174 1.433.372 1.942.205.526.478.972.923 1.417.444.445.89.719 1.416.923.51.198 1.09.333 1.942.372C5.555 15.99 5.827 16 8 16s2.444-.01 3.298-.048c.851-.04 1.434-.174 1.943-.372a3.916 3.916 0 0 0 1.416-.923c.445-.445.718-.891.923-1.417.197-.509.332-1.09.372-1.942C15.99 10.445 16 10.173 16 8s-.01-2.445-.048-3.299c-.04-.851-.175-1.433-.372-1.941a3.926 3.926 0 0 0-.923-1.417A3.911 3.911 0 0 0 13.24.42c-.51-.198-1.092-.333-1.943-.372C10.443.01 10.172 0 7.998 0h.003zm-.717 1.442h.718c2.136 0 2.389.007 3.232.046.78.035 1.204.166 1.486.275.373.145.64.319.92.599.28.28.453.546.598.92.11.281.24.705.275 1.485.039.843.047 1.096.047 3.231s-.008 2.389-.047 3.232c-.035.78-.166 1.203-.275 1.485a2.47 2.47 0 0 1-.599.919c-.28.28-.546.453-.92.598-.28.11-.704.24-1.485.276-.843.038-1.096.047-3.232.047s-2.39-.009-3.233-.047c-.78-.036-1.203-.166-1.485-.276a2.478 2.478 0 0 1-.92-.598 2.48 2.48 0 0 1-.6-.92c-.109-.281-.24-.705-.275-1.485-.038-.843-.046-1.096-.046-3.233 0-2.136.008-2.388.046-3.231.036-.78.166-1.204.276-1.486.145-.373.319-.64.599-.92.28-.28.546-.453.92-.598.282-.11.705-.24 1.485-.276.738-.034 1.024-.044 2.515-.045v.002zm4.988 1.328a.96.96 0 1 0 0 1.92.96.96 0 0 0 0-1.92zm-4.27 1.122a4.109 4.109 0 1 0 0 8.217 4.109 4.109 0 0 0 0-8.217zm0 1.441a2.667 2.667 0 1 1 0 5.334 2.667 2.667 0 0 1 0-5.334z"/>
              </svg>
            </SocialIcon>
            <SocialIcon href="https://www.youtube.com/starbucks" target="_blank" rel="noopener noreferrer">
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                <path d="M8.051 1.999h.089c.822.003 4.987.033 6.11.335a2.01 2.01 0 0 1 1.415 1.42c.101.38.172.883.22 1.402l.01.104.022.26.008.104c.065.914.073 1.77.074 1.957v.075c-.001.194-.01 1.108-.082 2.06l-.008.105-.009.104c-.05.572-.124 1.14-.235 1.558a2.007 2.007 0 0 1-1.415 1.42c-1.16.312-5.569.334-6.18.335h-.142c-.309 0-1.587-.006-2.927-.052l-.17-.006-.087-.004-.171-.007-.171-.007c-1.11-.049-2.167-.128-2.654-.26a2.007 2.007 0 0 1-1.415-1.419c-.111-.417-.185-.986-.235-1.558L.09 9.82l-.008-.104A31.4 31.4 0 0 1 0 7.68v-.123c.002-.215.01-.958.064-1.778l.007-.103.003-.052.008-.104.022-.26.01-.104c.048-.519.119-1.023.22-1.402a2.007 2.007 0 0 1 1.415-1.42c.487-.13 1.544-.21 2.654-.26l.17-.007.172-.006.086-.003.171-.007A99.788 99.788 0 0 1 7.858 2h.193zM6.4 5.209v4.818l4.157-2.408L6.4 5.209z"/>
              </svg>
            </SocialIcon>
            <SocialIcon href="https://twitter.com/starbucks" target="_blank" rel="noopener noreferrer">
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                <path d="M5.026 15c6.038 0 9.341-5.003 9.341-9.334 0-.14 0-.282-.006-.422A6.685 6.685 0 0 0 16 3.542a6.658 6.658 0 0 1-1.889.518 3.301 3.301 0 0 0 1.447-1.817 6.533 6.533 0 0 1-2.087.793A3.286 3.286 0 0 0 7.875 6.03a9.325 9.325 0 0 1-6.767-3.429 3.289 3.289 0 0 0 1.018 4.382A3.323 3.323 0 0 1 .64 6.575v.045a3.288 3.288 0 0 0 2.632 3.218 3.203 3.203 0 0 1-.865.115 3.23 3.23 0 0 1-.614-.057 3.283 3.283 0 0 0 3.067 2.277A6.588 6.588 0 0 1 .78 13.58a6.32 6.32 0 0 1-.78-.045A9.344 9.344 0 0 0 5.026 15z"/>
              </svg>
            </SocialIcon>
          </SocialIcons>
        </SocialSection>

        <LegalSection>
          <Link to="/privacy">Privacy Policy</Link>
          <Link to="/terms">Terms of Use</Link>
          <Link to="/ca-supply">CA Supply Chain Act</Link>
          <Link to="/accessibility">Accessibility</Link>
          <Link to="/notice">Cookie Preferences</Link>
        </LegalSection>

        <CopyrightText>
          © 2023 Starbucks Coffee Company. All rights reserved.
        </CopyrightText>
      </FooterSection>
    </FooterContainer>
  );
}

export default Footer; 