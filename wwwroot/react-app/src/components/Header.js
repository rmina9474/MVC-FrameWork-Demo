import React, { useState } from "react";
import { Link } from "react-router-dom";
import styled from "styled-components";

const HeaderContainer = styled.header`
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 0;
  z-index: 100;
  background-color: white;
`;

const NavContainer = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px 40px;
  max-width: 1440px;
  margin: 0 auto;

  @media (max-width: 1024px) {
    padding: 16px 24px;
  }

  @media (max-width: 768px) {
    padding: 16px;
  }
`;

const Logo = styled.div`
  img {
    height: 50px;
    width: auto;
  }
`;

const DesktopNav = styled.nav`
  display: flex;
  align-items: center;

  @media (max-width: 1024px) {
    display: none;
  }
`;

const NavLinks = styled.ul`
  display: flex;
  list-style: none;
  margin: 0;
  padding: 0;
`;

const NavItem = styled.li`
  margin: 0 20px;

  a {
    color: var(--starbucks-black);
    font-weight: 700;
    text-transform: uppercase;
    font-size: 14px;
    letter-spacing: 0.1em;
    padding: 10px 0;
    position: relative;

    &::after {
      content: "";
      position: absolute;
      bottom: 0;
      left: 0;
      width: 0;
      height: 2px;
      background-color: var(--starbucks-black);
      transition: width 0.3s ease;
    }

    &:hover::after {
      width: 100%;
    }
  }
`;

const LocationButton = styled.button`
  display: flex;
  align-items: center;
  margin-right: 40px;
  font-weight: 600;

  svg {
    margin-right: 8px;
  }
`;

const ActionButtons = styled.div`
  display: flex;
  align-items: center;
`;

const SignInButton = styled(Link)`
  border: 1px solid var(--starbucks-black);
  border-radius: 50px;
  padding: 7px 16px;
  margin-right: 16px;
  font-weight: 600;
  font-size: 14px;

  &:hover {
    background-color: rgba(0, 0, 0, 0.06);
  }
`;

const JoinNowButton = styled(Link)`
  background-color: var(--starbucks-black);
  color: white;
  border-radius: 50px;
  padding: 7px 16px;
  font-weight: 600;
  font-size: 14px;

  &:hover {
    background-color: rgba(0, 0, 0, 0.7);
  }
`;

const MobileMenuButton = styled.button`
  display: none;
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;

  @media (max-width: 1024px) {
    display: block;
  }
`;

const MobileMenu = styled.div`
  position: fixed;
  top: 0;
  right: ${({ isOpen }) => (isOpen ? "0" : "-100%")};
  width: 80%;
  max-width: 300px;
  height: 100vh;
  background-color: white;
  transition: right 0.3s ease;
  z-index: 1000;
  box-shadow: -2px 0 10px rgba(0, 0, 0, 0.1);
  overflow-y: auto;
  display: none;

  @media (max-width: 1024px) {
    display: block;
  }
`;

const MobileMenuHeader = styled.div`
  display: flex;
  justify-content: flex-end;
  padding: 20px;
`;

const CloseButton = styled.button`
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
`;

const MobileNavLinks = styled.ul`
  list-style: none;
  margin: 0;
  padding: 0;
`;

const MobileNavItem = styled.li`
  border-bottom: 1px solid var(--starbucks-gray);

  a {
    display: block;
    padding: 16px 20px;
    font-weight: 600;
    font-size: 16px;
  }
`;

const MobileActionButtons = styled.div`
  padding: 20px;
  display: flex;
  flex-direction: column;
  gap: 12px;
`;

const Overlay = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  z-index: 900;
  opacity: ${({ isOpen }) => (isOpen ? "1" : "0")};
  visibility: ${({ isOpen }) => (isOpen ? "visible" : "hidden")};
  transition:
    opacity 0.3s ease,
    visibility 0.3s ease;
`;

function Header() {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  const toggleMobileMenu = () => {
    setMobileMenuOpen(!mobileMenuOpen);
    document.body.style.overflow = !mobileMenuOpen ? "hidden" : "";
  };

  return (
    <HeaderContainer>
      <NavContainer>
        <Logo>
          <Link to="/">
            <img src="/images/starbucks-logo.svg" alt="Starbucks" />
          </Link>
        </Logo>

        <DesktopNav>
          <NavLinks>
            <NavItem>
              <Link to="/menu">Menu</Link>
            </NavItem>
            <NavItem>
              <Link to="/rewards">Rewards</Link>
            </NavItem>
            <NavItem>
              <Link to="/gift-cards">Gift Cards</Link>
            </NavItem>
          </NavLinks>
        </DesktopNav>

        <ActionButtons>
          <LocationButton>
            <svg
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M12 2C8.13 2 5 5.13 5 9C5 14.25 12 22 12 22C12 22 19 14.25 19 9C19 5.13 15.87 2 12 2ZM12 11.5C10.62 11.5 9.5 10.38 9.5 9C9.5 7.62 10.62 6.5 12 6.5C13.38 6.5 14.5 7.62 14.5 9C14.5 10.38 13.38 11.5 12 11.5Z"
                fill="currentColor"
              />
            </svg>
            Find a store
          </LocationButton>
          <SignInButton to="/signin">Sign in</SignInButton>
          <JoinNowButton to="/join">Join now</JoinNowButton>

          <MobileMenuButton onClick={toggleMobileMenu}>
            <svg
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M3 18H21V16H3V18ZM3 13H21V11H3V13ZM3 6V8H21V6H3Z"
                fill="currentColor"
              />
            </svg>
          </MobileMenuButton>
        </ActionButtons>
      </NavContainer>

      <MobileMenu isOpen={mobileMenuOpen}>
        <MobileMenuHeader>
          <CloseButton onClick={toggleMobileMenu}>
            <svg
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M19 6.41L17.59 5L12 10.59L6.41 5L5 6.41L10.59 12L5 17.59L6.41 19L12 13.41L17.59 19L19 17.59L13.41 12L19 6.41Z"
                fill="currentColor"
              />
            </svg>
          </CloseButton>
        </MobileMenuHeader>

        <MobileNavLinks>
          <MobileNavItem>
            <Link to="/menu" onClick={toggleMobileMenu}>
              Menu
            </Link>
          </MobileNavItem>
          <MobileNavItem>
            <Link to="/rewards" onClick={toggleMobileMenu}>
              Rewards
            </Link>
          </MobileNavItem>
          <MobileNavItem>
            <Link to="/gift-cards" onClick={toggleMobileMenu}>
              Gift Cards
            </Link>
          </MobileNavItem>
        </MobileNavLinks>

        <MobileActionButtons>
          <SignInButton
            to="/signin"
            onClick={toggleMobileMenu}
            style={{ display: "inline-block", textAlign: "center" }}
          >
            Sign in
          </SignInButton>
          <JoinNowButton
            to="/join"
            onClick={toggleMobileMenu}
            style={{ display: "inline-block", textAlign: "center" }}
          >
            Join now
          </JoinNowButton>

          <LocationButton style={{ marginTop: "20px", marginRight: "0" }}>
            <svg
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M12 2C8.13 2 5 5.13 5 9C5 14.25 12 22 12 22C12 22 19 14.25 19 9C19 5.13 15.87 2 12 2ZM12 11.5C10.62 11.5 9.5 10.38 9.5 9C9.5 7.62 10.62 6.5 12 6.5C13.38 6.5 14.5 7.62 14.5 9C14.5 10.38 13.38 11.5 12 11.5Z"
                fill="currentColor"
              />
            </svg>
            Find a store
          </LocationButton>
        </MobileActionButtons>
      </MobileMenu>

      <Overlay isOpen={mobileMenuOpen} onClick={toggleMobileMenu} />
    </HeaderContainer>
  );
}

export default Header;
